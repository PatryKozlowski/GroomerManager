using FluentValidation;
using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Auth;

public abstract class CreateUserCommand
{
        public class Request : IRequest<Result>
    {
       public required string Email { get; set; }
       public required string Password { get; set; }
       public required string RepeatPassword { get; set; }
    }
    
    public class Result
    {
        public required int UserId { get; set; }
    }
    
    public class Handler(
        IApplicationDbContext applicationDbContext, 
        IPasswordManager passwordManager
        ) : 
        BaseCommandHandler(
            applicationDbContext
            ), 
        IRequestHandler<Request, Result>
    {
        private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var userExist = await _applicationDbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken: cancellationToken);

            if (userExist)
            {
                throw new ErrorException("AccountWithThisEmailAlreadyExists");
            }

            var user = new User
            {
                Email = request.Email,
                HashedPassword = ""
            };
            
            user.HashedPassword = passwordManager.HashPassword(request.RepeatPassword);

            _applicationDbContext.Users.Add(user);
            
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return new Result()
            {
                UserId = user.Id
            };
        }
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("EmailIsRequired")
                .EmailAddress().WithMessage("InvalidEmailAddress");
            
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("PasswordIsRequired")
                .MinimumLength(8).WithMessage("PasswordTooShort")
                .Matches("[A-Z]").WithMessage("PasswordMustContainUppercase")
                .Matches("[a-z]").WithMessage("PasswordMustContainLowercase")
                .Matches("[0-9]").WithMessage("PasswordMustContainNumber")
                .Matches("[^a-zA-Z0-9]").WithMessage("PasswordMustContainSpecialCharacter");
            
            RuleFor(x => x.RepeatPassword)
                .NotEmpty().WithMessage("RepeatPasswordIsRequired")
                .Equal(x => x.Password).WithMessage("PasswordsDoNotMatch");
        }
    }
}
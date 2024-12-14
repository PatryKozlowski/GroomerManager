using FluentValidation;
using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using GroomerManager.Shared.DTOs.Request;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Auth;

public abstract class LoginCommand
{
    public class Request : LoginRequestDto, IRequest<UserDto>
    {
    }
    
    public class Handler(
        IApplicationDbContext applicationDbContext,
        IPasswordManager passwordManager
        ) : 
        BaseCommandHandler(
            applicationDbContext
            ), 
        IRequestHandler<Request, UserDto>
    {
        private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task<UserDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = await _applicationDbContext.Users
                .Include(u => u.Roles)        
                .ThenInclude(ur => ur.Role)   
                .FirstOrDefaultAsync(u => u.Email == request.Email,
                cancellationToken);

            if (user == null)
            {
                throw new ErrorException("InvalidEmailOrPassword");
            }
            
            if (passwordManager.VerifyPassword(user.HashedPassword, request.Password))
            {
                return new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = user.Roles
                };
            }

            throw new ErrorException("InvalidEmailOrPassword");
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
        }
    }
}
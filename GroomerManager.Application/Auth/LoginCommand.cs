using System.Data;
using FluentValidation;
using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using GroomerManager.Domain.Entities;
using GroomerManager.Shared.DTOs.Request;
using GroomerManager.Shared.DTOs.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Auth;

public abstract class LoginCommand
{
    public class Request : LoginRequestDto, IRequest<LoginResponseDto>
    {
    }
    
    public class Handler(
        IApplicationDbContext applicationDbContext,
        IJwtManager jwtManager,
        IPasswordManager passwordManager,
        IDateTime dateTime
        ) : 
        BaseCommandHandler(
            applicationDbContext
            ), 
        IRequestHandler<Request, LoginResponseDto>
    {
        private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task<LoginResponseDto> Handle(Request request, CancellationToken cancellationToken)
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
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = user.Roles
                };

                var token = jwtManager.GenerateUserToken(userDto, false);
                var refreshToken = jwtManager.GenerateUserToken(userDto, true);
                
                var existingToken = await _applicationDbContext.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.UserId == user.Id, cancellationToken);

                if (existingToken != null)
                {
                    existingToken.Token = refreshToken;
                }
                else
                {
                    var newToken = new RefreshToken
                    {
                        UserId = user.Id,
                        Token = refreshToken
                    };
                    _applicationDbContext.RefreshTokens.Add(newToken);
                }

                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return new LoginResponseDto
                {
                    Token = token,
                    TokenExpired = dateTime.Now.AddMinutes(24*60*30).ToUnixTimeSeconds(),
                    RefreshToken = refreshToken
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
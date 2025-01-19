using FluentValidation;
using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using GroomerManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Auth;

public abstract class LoginCommand
{
    public class Request : LoginRequestDto, IRequest<LoginResponseDto>
    {
    }
    
    
    public class Handler : BaseCommandHandler, IRequestHandler<Request, LoginResponseDto>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly IDateTime _dateTime;
        private readonly IPasswordManager _passwordManager;
        private readonly IJwtManager _jwtManager;

        public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider, IDateTime dateTime, IPasswordManager passwordManager, IJwtManager jwtManager) : base(groomerManagerDb, currentSalonProvider)
        {
            _groomerManagerDb = groomerManagerDb;
            _dateTime = dateTime;
            _passwordManager = passwordManager;
            _jwtManager = jwtManager;
        }
        

        public async Task<LoginResponseDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = await _groomerManagerDb.Users
                .Include(u => u.Roles)        
                .ThenInclude(ur => ur.Role)   
                .FirstOrDefaultAsync(u => u.Email == request.Email,
                    cancellationToken);

            if (user == null)
            {
                throw new ErrorException("InvalidEmailOrPassword");
            }
            
            if (_passwordManager.VerifyPassword(user.HashedPassword, request.Password))
            {
                var userDto = new UserDto(
                    user.Id,
                    user.Email,
                    user.Roles.Select(r => r.Role.Name).ToList()
                );

                var token = _jwtManager.GenerateUserToken(userDto, false);
                var refreshToken = _jwtManager.GenerateUserToken(userDto, true);
                
                var existingToken = await _groomerManagerDb.RefreshTokens
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
                    _groomerManagerDb.RefreshTokens.Add(newToken);
                }

                await _groomerManagerDb.SaveChangesAsync(cancellationToken);

                return new LoginResponseDto
                {
                    Token = token,
                    TokenExpired = _dateTime.Now.AddMinutes(24*60*30).ToUnixTimeSeconds(),
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
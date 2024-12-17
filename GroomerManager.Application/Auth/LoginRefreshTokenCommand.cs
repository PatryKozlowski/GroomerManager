using FluentValidation;
using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using GroomerManager.Domain.Entities;
using GroomerManager.Shared.DTOs.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Auth;

public abstract class LoginRefreshTokenCommand
{
    public class Request : IRequest<LoginResponseDto>
    {
        public required string RefreshToken { get; set; }
    }
    
    public class Handler(
        IApplicationDbContext applicationDbContext,
        IJwtManager jwtManager,
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
            var user = await _applicationDbContext.RefreshTokens
                .Where(n => n.Token == request.RefreshToken)
                .Include(n => n.User)
                    .ThenInclude(u => u.Roles)
                    .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(cancellationToken);
            

            if (user == null)
            {
                throw new UnauthorizedException();
            }
            
            var userDto = new UserDto
            {
                Id = user.User.Id,
                Email = user.User.Email,
                Roles = user.User.Roles
            };

            var token = jwtManager.GenerateUserToken(userDto, false);
            var refreshToken = jwtManager.GenerateUserToken(userDto, true);
            
            var existingToken = await _applicationDbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == user.User.Id, cancellationToken);

            if (existingToken != null)
            {
                existingToken.Token = refreshToken;
            }
            else
            {
                var newToken = new RefreshToken
                {
                    UserId = user.User.Id,
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
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshTokenIsRequired");
        }
    }
}
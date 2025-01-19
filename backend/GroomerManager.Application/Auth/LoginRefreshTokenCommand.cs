using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using GroomerManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Auth;

public abstract class LoginRefreshTokenCommand
{
    public class Request : IRequest<LoginResponseDto>
    {
    }
    
    public class Handler :
        BaseCommandHandler,
        IRequestHandler<Request, LoginResponseDto>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly IAuthenticationDataProvider _authenticationData;
        private readonly IJwtManager _jwtManager;
        private readonly IDateTime _dateTime;

        public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider, IAuthenticationDataProvider authenticationData, IJwtManager jwtManager, IDateTime dateTime) : base(groomerManagerDb, currentSalonProvider)
        {
            _groomerManagerDb = groomerManagerDb;
            _authenticationData = authenticationData;
            _jwtManager = jwtManager;
            _dateTime = dateTime;
        }

        public async Task<LoginResponseDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var refreshTokenFromCookie = _authenticationData.GetUserRefreshTokenFromCookie();
            
            var user = await _groomerManagerDb.RefreshTokens
                .Where(n => n.Token == refreshTokenFromCookie)
                .Include(n => n.User)
                    .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(cancellationToken);
            

            if (user == null)
            {
                throw new UnauthorizedException();
            }
            
            var userDto = new UserDto
            {
                Id = user.User.Id,
                Email = user.User.Email,
                Role = user.User.Role
            };

            var token = _jwtManager.GenerateUserToken(userDto, false);
            var refreshToken = _jwtManager.GenerateUserToken(userDto, true);
            
            var existingToken = await _groomerManagerDb.RefreshTokens
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
    }
}
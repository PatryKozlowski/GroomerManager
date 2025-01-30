using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Auth;

public abstract class GetUserInfoCommand
{
    public class Request : IRequest<LoggedInUserResponseDto>
    {
    }
    
    public class Handler: IRequestHandler<Request, LoggedInUserResponseDto>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly ICurrentSalonProvider _currentSalonProvider;
        private readonly IAuthenticationDataProvider _authenticationData;

        public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider, IAuthenticationDataProvider authenticationData) : base()
        {
            _groomerManagerDb = groomerManagerDb;
            _currentSalonProvider = currentSalonProvider;
            _authenticationData = authenticationData;
        }


        public async Task<LoggedInUserResponseDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var userId = _authenticationData.GetUserId();
            
            if (userId == null)
            {
                throw new UnauthorizedException();
            }

            var user = await _groomerManagerDb.Users
                .Include(u => u.Role)
                .Include(u => u.UserInfo)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            
            if (user == null)
            {
                throw new ErrorException("UserNotFound");
            }
            

            return new LoggedInUserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role.Name,
                FullName = user.UserInfo.UserName.ToString()
            };
        }
    }
}
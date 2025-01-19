using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Auth;

public abstract class LogoutCommand
{
    public class Request : IRequest<LogoutResponseDto>
    {
    }
    
    
    public class Handler : 
        BaseCommandHandler, 
        IRequestHandler<Request, LogoutResponseDto>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly IAuthenticationDataProvider _authenticationDataProvider;

        public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider, IAuthenticationDataProvider authenticationDataProvider) : base(groomerManagerDb, currentSalonProvider)
        {
            _groomerManagerDb = groomerManagerDb;
            _authenticationDataProvider = authenticationDataProvider;
        }

        public async Task<LogoutResponseDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var userId = _authenticationDataProvider.GetUserId();

            if (userId == null)
            {
                throw new UnauthorizedException();
            }
            
            return new LogoutResponseDto
            {
                Message = "LogoutSuccessful"
            };
        }
    }
}
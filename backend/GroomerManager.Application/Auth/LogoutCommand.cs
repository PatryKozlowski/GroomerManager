using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Auth;

public abstract class LogoutCommand
{
    public class Request : IRequest<MessageResponseDto>
    {
    }
    
    
    public class Handler : 
        BaseCommandHandler, 
        IRequestHandler<Request, MessageResponseDto>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly IAuthenticationDataProvider _authenticationDataProvider;

        public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider, IAuthenticationDataProvider authenticationDataProvider) : base(groomerManagerDb, currentSalonProvider)
        {
            _groomerManagerDb = groomerManagerDb;
            _authenticationDataProvider = authenticationDataProvider;
        }

        public async Task<MessageResponseDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var userId = _authenticationDataProvider.GetUserId();

            if (userId == null)
            {
                throw new UnauthorizedException();
            }
            
            return new MessageResponseDto
            {
                Message = "LogoutSuccessful"
            };
        }
    }
}
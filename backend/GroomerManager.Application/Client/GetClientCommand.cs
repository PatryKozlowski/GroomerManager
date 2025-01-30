using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Client;

public abstract class GetClientCommand
{
     public class Request : IRequest<ClientResponseDto>
    {
        public Guid SalonId { get; set; }
        public Guid ClientId { get; set; }
    }

    public class Handler : BaseCommandHandler, IRequestHandler<Request, ClientResponseDto>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly ICurrentSalonProvider _currentSalonProvider;

        public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider) 
            : base(groomerManagerDb, currentSalonProvider)
        {
            _groomerManagerDb = groomerManagerDb;
            _currentSalonProvider = currentSalonProvider;
        }

        public async Task<ClientResponseDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var salon = await _currentSalonProvider.GetAuthenticatedSalon(request.SalonId);
            if (salon == null)
            {
                throw new Exception("SalonNotFoundOrUnauthorized");
            }

            var client = await _groomerManagerDb.Clients
                .Where(c => c.SalonId == salon.Id && c.Id == request.ClientId)
                .FirstOrDefaultAsync(cancellationToken);


            if (client == null)
            {
                throw new ErrorException("ClientNotFound");
            }
            
            return new ClientResponseDto
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                PhoneNumber = client.PhoneNumber,
                Email = client.Email,
            };
        }

    }
}
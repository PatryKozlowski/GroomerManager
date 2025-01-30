using FluentValidation;
using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Client;

public abstract class DeleteClientCommand
{
     public class Request : IRequest<MessageResponseDto>
    {
        public Guid SalonId { get; set; }
        public Guid ClientId { get; set; }
    }

    public class Handler : BaseCommandHandler, IRequestHandler<Request, MessageResponseDto>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly ICurrentSalonProvider _currentSalonProvider;

    public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider) : base(groomerManagerDb, currentSalonProvider)
    {
        _groomerManagerDb = groomerManagerDb;
        _currentSalonProvider = currentSalonProvider;
    }

    public async Task<MessageResponseDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var salon = await _currentSalonProvider.GetAuthenticatedSalon(request.SalonId);
            
            var client = await _groomerManagerDb.Clients
                .Where(c => c.SalonId == salon.Id && c.Id == request.ClientId)
                .FirstOrDefaultAsync(cancellationToken);

            if (client == null)
            {
                throw new ErrorException("ClientNotFound");
            }
            

            _groomerManagerDb.Clients.Remove(client);
            
            await _groomerManagerDb.SaveChangesAsync(cancellationToken);

            return new MessageResponseDto
            {
                Message = "ClientDeleted"
            };
        }
    }
    
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty()
                .WithMessage("ClientIdIsRequired");
            
            RuleFor(x => x.SalonId)
                .NotEmpty()
                .WithMessage("SalonIdIsRequired");
        }
    }
}
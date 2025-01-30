using GroomerManager.Application.Client;
using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Note.Client;

public abstract class GetClientNoteCommand
{
    public class Request : IRequest<List<ClientNoteResponseDto>>
    {
        public Guid SalonId { get; set; }
        public Guid ClientId { get; set; }
    }

    public class Handler : BaseCommandHandler, IRequestHandler<Request, List<ClientNoteResponseDto>>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly ICurrentSalonProvider _currentSalonProvider;

        public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider) 
            : base(groomerManagerDb, currentSalonProvider)
        {
            _groomerManagerDb = groomerManagerDb;
            _currentSalonProvider = currentSalonProvider;
        }

        public async Task<List<ClientNoteResponseDto>> Handle(Request request, CancellationToken cancellationToken)
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

            var note = await _groomerManagerDb.ClientNotes
                .Where(n => n.ClientId == client.Id)
                .OrderByDescending(n => n.Created)
                .ToListAsync(cancellationToken);

            if (note == null)
            {
                throw new ErrorException("NoteNotFound");
            }
            

            return note.Select(n => new ClientNoteResponseDto
            {
                Id = n.Id,
                Text = n.Text,
                Created = n.Created,
                CreatedBy = n.CreatedBy,
            }).ToList();
        }
    }
}
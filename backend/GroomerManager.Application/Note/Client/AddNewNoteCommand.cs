using FluentValidation;
using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using GroomerManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Note.Client;

public abstract class AddNewNoteCommand
{
    public class Request : AddNoteForClientRequestDto, IRequest<ClientNoteResponseDto>
    {
        public Guid SalonId { get; set; }
        public Guid ClientId { get; set; }
    }

    public class Handler : BaseCommandHandler, IRequestHandler<Request, ClientNoteResponseDto>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly ICurrentSalonProvider _currentSalonProvider;
        private readonly IAuthenticationDataProvider _authenticationData;

        public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider, IAuthenticationDataProvider authenticationData)
            : base(groomerManagerDb, currentSalonProvider)
        {
            _groomerManagerDb = groomerManagerDb;
            _currentSalonProvider = currentSalonProvider;
            _authenticationData = authenticationData;
        }

        public async Task<ClientNoteResponseDto> Handle(Request request, CancellationToken cancellationToken)
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

            var userEmail = _authenticationData.GetUserEmail();
                
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new UnauthorizedException();
            }
                
            var user = await _groomerManagerDb.Users
                .Where(u => u.Email == userEmail)
                .Include(u => u.UserInfo)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new UnauthorizedException();
            }
    
            var note = new ClientNote
            {
                ClientId = client.Id,
                Text = request.Note,
                Created = DateTime.UtcNow,
                CreatedBy = user.UserInfo.UserName.ToString(),
                Client = client
            };
    
            _groomerManagerDb.ClientNotes.Add(note);
            await _groomerManagerDb.SaveChangesAsync(cancellationToken);
    
            return new ClientNoteResponseDto
            {
                Id = note.Id,
                Text = note.Text,
                Created = note.Created,
                CreatedBy = note.CreatedBy,
            };
        }
    }
    
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Note)
                .NotEmpty()
                .WithMessage("TextIsRequired")
                .MaximumLength(255)
                .WithMessage("InvalidTextLength");
        }
    }
}
using FluentValidation;
using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Client;

public abstract class EditClientCommand
{
    public class Request : EditClientRequestDto, IRequest<NewClientResponseDto>
    {
        public Guid SalonId { get; set; }
    }

    public class Handler : BaseCommandHandler, IRequestHandler<Request, NewClientResponseDto>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly ICurrentSalonProvider _currentSalonProvider;

    public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider) : base(groomerManagerDb, currentSalonProvider)
    {
        _groomerManagerDb = groomerManagerDb;
        _currentSalonProvider = currentSalonProvider;
    }

    public async Task<NewClientResponseDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var salon = await _currentSalonProvider.GetAuthenticatedSalon(request.SalonId);
            
            var client = await _groomerManagerDb.Clients
                .Where(c => c.SalonId == salon.Id && c.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (client == null)
            {
                throw new ErrorException("ClientNotFound");
            }
            
            client.FirstName = request.FirstName;
            client.LastName = request.LastName;
            client.Email = request.Email;
            client.PhoneNumber = request.PhoneNumber;

            _groomerManagerDb.Clients.Update(client);
            
            await _groomerManagerDb.SaveChangesAsync(cancellationToken);

            return new NewClientResponseDto
            {
                ClientId = client.Id
            };
        }
    }
    
    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("FirstNameIsRequired")
                .MaximumLength(50)
                .WithMessage("InvalidFirstNameLength");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("LastNameIsRequired")
                .MaximumLength(50)
                .WithMessage("InvalidLastNameLength");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("PhoneNumberIsRequired")
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("InvalidPhoneNumberFormat");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("InvalidEmailAddress")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }
}
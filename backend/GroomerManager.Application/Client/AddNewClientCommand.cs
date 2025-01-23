using FluentValidation;
using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;

namespace GroomerManager.Application.Client;

public abstract class AddNewClientCommand
{
    public class Request : NewClientRequestDto, IRequest<NewClientResponseDto>
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
            
            var isClientExist = _groomerManagerDb.Clients
                .Where(c => c.SalonId == salon.Id)
                .Any(x => x.PhoneNumber == request.PhoneNumber);
            
            if (isClientExist)
            {
                throw new ErrorException("ClientAlreadyExist");
            }
            
            var client = new Domain.Entities.Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                SalonId = salon.Id,
                Salon = salon
            };

            _groomerManagerDb.Clients.Add(client);
            
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
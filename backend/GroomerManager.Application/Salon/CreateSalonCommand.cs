using FluentValidation;
using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Salon;

public abstract class CreateSalonCommand
{
    public class Request : AddSalonRequestDto, IRequest<AddSalonResponseDto>
    {
       
    }
    
    public class Handler : BaseCommandHandler,  IRequestHandler<Request, AddSalonResponseDto>
    {
        private readonly IAuthenticationDataProvider _authenticationData;
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly IBlobService _blobService;
        
        public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider, IAuthenticationDataProvider authenticationData, IBlobService blobService) : base(groomerManagerDb, currentSalonProvider)
        {
            _authenticationData = authenticationData;
            _groomerManagerDb = groomerManagerDb;
            _blobService = blobService;
        }

        public async Task<AddSalonResponseDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var userId = _authenticationData.GetUserId();

            if (userId == null)
            {
                throw new UnauthorizedException();
            }

            var user = await _groomerManagerDb.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId.Value, cancellationToken);
            
            if (user == null)
            {
                throw new ErrorException("UserDoesNotExist");
            }
            
            var isSalonExist = await _groomerManagerDb.Salons.AnyAsync(s => s.Name == request.Name, cancellationToken);
            
            if (isSalonExist)
            {
                throw new ErrorException("SalonAlreadyExist");
            }
            
            using Stream stream = request.Logo.OpenReadStream();

            var (logoGlobPath, logoId) = await _blobService.UploadAsync(stream, request.Logo.ContentType, cancellationToken);

            if (logoGlobPath == null)
            {
                throw new ErrorException("ErrorOccuredDuringBlobLog");
            }

            var newSalon = new Domain.Entities.Salon
            {
                Name = request.Name,
                LogoPath = logoGlobPath.ToString(),
                LogoId = logoId,
                OwnerId = user.Id,
                Owner = user
            };

            _groomerManagerDb.Salons.Add(newSalon);
            
            var userSalon = new Domain.Entities.UserSalon
            {
                UserId = user.Id,
                SalonId = newSalon.Id,
                Role = user.Role.Name
            };

            _groomerManagerDb.UserSalons.Add(userSalon);
            
            await _groomerManagerDb.SaveChangesAsync(cancellationToken);

            return new AddSalonResponseDto()
            {
                SalonId = newSalon.Id,
                LogoPath = newSalon.LogoPath,
                Name = newSalon.Name
            };
        }
        
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("SalonNameIsRequired");

                RuleFor(x => x.Logo)
                    .NotEmpty().WithMessage("SalonLogoIsRequired");
            }
        }
    }
}
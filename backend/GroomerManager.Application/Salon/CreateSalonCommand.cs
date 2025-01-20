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

            var user = await _groomerManagerDb.Users.FirstOrDefaultAsync(u => u.Id == userId.Value);
            
            if (user == null)
            {
                throw new ErrorException("UserDoesNotExist");
            }
            
            using Stream stream = request.Logo.OpenReadStream();

            var (logoGlobPath, logoId) = await _blobService.UploadAsync(stream, request.Logo.ContentType);

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

            await _groomerManagerDb.SaveChangesAsync();

            return new AddSalonResponseDto()
            {
                SalonId = newSalon.Id,
                LogoPath = newSalon.LogoPath
            };
        }
    }
}
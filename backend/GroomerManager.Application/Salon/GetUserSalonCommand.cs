using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UnauthorizedException = GroomerManager.Application.Common.Exceptions.UnauthorizedException;

namespace GroomerManager.Application.Salon;

public abstract class GetUserSalonCommand
{
    public class Request : IRequest<List<SalonResponseDto>>
    {
        
    }

    public class Handler : BaseCommandHandler,  IRequestHandler<Request, List<SalonResponseDto>>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly IAuthenticationDataProvider _authenticationDataProvider;
        
        public Handler(IGroomerManagerDbContext groomerManagerDb, IAuthenticationDataProvider authenticationDataProvider, ICurrentSalonProvider currentSalonProvider) : base(groomerManagerDb, currentSalonProvider)
        {
            _groomerManagerDb = groomerManagerDb;
            _authenticationDataProvider = authenticationDataProvider;
        }

        public async Task<List<SalonResponseDto>> Handle(Request request, CancellationToken cancellationToken)
        {
            var userId = _authenticationDataProvider.GetUserId();

            if (userId == null)
            {
                throw new UnauthorizedException();
            }
            
            var user = await _groomerManagerDb.Users
                .Include(u => u.Role) 
                .Include(u => u.UserSalons) 
                .ThenInclude(us => us.Salon) 
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new ErrorException("UserNotFound.");
            }

            List<SalonResponseDto> salonDtos;

            if (user.Role.Name == "Owner")
            {
                var ownerSalons = await _groomerManagerDb.Salons
                    .Where(s => s.OwnerId == userId) // Właściciel salonu
                    .ToListAsync(cancellationToken);

                salonDtos = ownerSalons.Select(salon => new SalonResponseDto
                {
                    Id = salon.Id,
                    Name = salon.Name,
                    LogoPath = salon.LogoPath
                }).ToList();
            }
            else
            {
                var employeeSalons = user.UserSalons.Select(us => us.Salon);

                salonDtos = employeeSalons.Select(salon => new SalonResponseDto
                {
                    Id = salon.Id,
                    Name = salon.Name,
                    LogoPath = salon.LogoPath
                }).ToList();
            }
            
            return salonDtos;
        }
    }
}
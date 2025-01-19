using GroomerManager.Application.Common.Exceptions;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Common.Services;

public class CurrentSalonProvider : ICurrentSalonProvider
{
    private readonly IAuthenticationDataProvider _authenticationDataProvider;
    private readonly IGroomerManagerDbContext _groomerManagerDb;
    
    public CurrentSalonProvider(IAuthenticationDataProvider authenticationDataProvider, IGroomerManagerDbContext groomerManagerDbContext)
    {
        _authenticationDataProvider = authenticationDataProvider;
        _groomerManagerDb = groomerManagerDbContext;
    }

    public async Task<List<Guid>> GetSalonId()
    {
        var userId = _authenticationDataProvider.GetUserId();

        if (userId != null)
        {
            return await _groomerManagerDb.UserSalons
                .Where(us => us.UserId == userId.Value)
                .Select(us => us.SalonId)
                .ToListAsync();
        }

        return new List<Guid>();
    }

    public async Task<Salon> GetAuthenticatedSalon(Guid salonId)
    {
        var userId = _authenticationDataProvider.GetUserId();

        if (userId == null)
        {
            throw new UnauthorizedException();
        }
        
        var isAuthorized = await _groomerManagerDb.UserSalons
            .AnyAsync(us => us.UserId == userId.Value && us.SalonId == salonId);

        if (!isAuthorized)
        {
            throw new UnauthorizedException("NoAccessToThisSalon");
        }
        
        var salon = await _groomerManagerDb.Salons.FirstOrDefaultAsync(s => s.Id == salonId);

        if (salon == null)
        {
            throw new ErrorException("SalonDoesNotExist");
        }

        return salon;
    }
}
using GroomerManager.Domain.Entities;

namespace GroomerManager.Application.Common.Interfaces;

public interface ICurrentAccountProvider
{
    Task<List<Guid>> GetSalonId();
    Task<Salon> GetAuthenticatedSalon(Guid salonId);
}
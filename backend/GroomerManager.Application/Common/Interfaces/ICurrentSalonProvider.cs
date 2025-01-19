using GroomerManager.Domain.Entities;

namespace GroomerManager.Application.Common.Interfaces;

public interface ICurrentSalonProvider
{
    Task<List<Guid>> GetSalonId();
    Task<Salon> GetAuthenticatedSalon(Guid salonId);
}
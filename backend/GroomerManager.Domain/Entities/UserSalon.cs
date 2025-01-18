using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Entities;

public class UserSalon : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid SalonId { get; set; }
    public Salon Salon { get; set; } = null!;
    public string Role { get; set; } = null!;
}
using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Entities;

public class User : BaseEntity
{
    public required string Email { get; set; }
    public required string HashedPassword { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public ICollection<UserRole> Roles { get; set; } = [];
    public RefreshToken? RefreshToken { get; set; }
    public ICollection<UserSalon> UserSalons { get; set; }
}
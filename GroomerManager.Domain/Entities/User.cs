using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Entities;

public class User : BaseEntity
{
    public required string Email { get; set; }
    public required string HashedPassword { get; set; }
    public ICollection<UserRole> Roles { get; set; } = [];
}
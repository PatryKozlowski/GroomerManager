using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Entities;

public class UserRole : BaseEntity
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public User User { get; set; } = null!;
}
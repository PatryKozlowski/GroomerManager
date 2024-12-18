using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public int UserId { get; set; }
    public required string Token { get; set; }
    public User User { get; set; } = null!;
}

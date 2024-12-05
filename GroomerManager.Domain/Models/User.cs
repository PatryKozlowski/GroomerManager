using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Models;

public class User : BaseModel
{
    public required string Email { get; set; }
    public required string HasPassword { get; set; }
    public ICollection<AccountUser> AccountUsers { get; set; } = [];
}

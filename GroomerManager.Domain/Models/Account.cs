using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Models;

public class Account : BaseModel
{
    public required string Name { get; set; }
    public ICollection<AccountUser> AccountUsers { get; set; } = [];
}

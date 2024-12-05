using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Models;

public class AccountUser : BaseModel
{
    public int AccountId { get; set; }
    public Account Account { get; set; } = default!;
    public int UserId { get; set; }
    public User User { get; set; } = default!;
}

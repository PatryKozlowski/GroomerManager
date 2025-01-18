using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Entities;

public class ClientNote : BaseEntity
{
    public required string Text { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
}
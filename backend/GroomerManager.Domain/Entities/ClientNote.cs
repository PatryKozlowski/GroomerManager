using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Entities;

public class ClientNote : BaseEntity
{
    public required string Text { get; set; }
    public Guid ClientId { get; set; }
    public required Client Client { get; set; }
}
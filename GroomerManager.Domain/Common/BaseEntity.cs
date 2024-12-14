namespace GroomerManager.Domain.Common;

public class BaseEntity
{
    public int Id { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTimeOffset? Modified { get; set; }
    public int StatusId { get; set; }
    public string InactivatedBy { get; set; } = string.Empty;
    public DateTimeOffset? Inactivated { get; set; }
}
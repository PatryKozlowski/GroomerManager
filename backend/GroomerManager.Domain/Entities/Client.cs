using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Entities;

public class Client : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public Guid SalonId { get; set; }
    public  required Salon Salon { get; set; }
    public ICollection<ClientNote> Notes { get; private set; } = new List<ClientNote>();
}
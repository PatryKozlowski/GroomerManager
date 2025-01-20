using GroomerManager.Domain.Common;

namespace GroomerManager.Domain.Entities;

public class Salon : BaseEntity
{
    public required string Name { get; set; }
    public required string LogoPath { get; set; }
    public required Guid LogoId { get; set; }
    public string? Address { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public ICollection<UserSalon> UserSalons { get; set; } = null!;
    public ICollection<Client> Clients { get; set; } = new List<Client>();
} 
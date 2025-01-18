using GroomerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Common.Interfaces;

public interface IGroomerManagerDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<Domain.Entities.Client> Clients { get; set; }
    DbSet<ClientNote> ClientNotes { get; set; }
    DbSet<Salon> Salons { get; set; }
    DbSet<UserSalon> UserSalons { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
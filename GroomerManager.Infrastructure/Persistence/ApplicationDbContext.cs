using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.Common;
using GroomerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IDateTime dateTime
) : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userEmail = "System";

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = dateTime.Now;
                    entry.Entity.CreatedBy = userEmail;
                    entry.Entity.StatusId = 1;
                    break;

                case EntityState.Modified:
                    entry.Entity.Modified = dateTime.Now;
                    entry.Entity.ModifiedBy = userEmail;
                    break;

                case EntityState.Deleted:
                    entry.Entity.Modified = dateTime.Now;
                    entry.Entity.Inactivated = dateTime.Now;
                    entry.Entity.InactivatedBy = userEmail;
                    entry.Entity.StatusId = 0;
                    entry.State = EntityState.Modified;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
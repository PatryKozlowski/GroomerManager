using GroomerManager.Domain.Common;
using GroomerManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountUser> AccountUsers => Set<AccountUser>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseModel>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTimeOffset.UtcNow;
                    entry.Entity.StatusId = 1;
                    break;

                case EntityState.Modified:
                    entry.Entity.Modified = DateTimeOffset.UtcNow;
                    break;

                case EntityState.Deleted:
                    entry.Entity.Modified = DateTimeOffset.UtcNow;
                    entry.Entity.Inactivated = DateTimeOffset.UtcNow;
                    entry.Entity.StatusId = 0;
                    entry.State = EntityState.Modified;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}

using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.Common;
using GroomerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Infrastructure.Persistence;

public class GroomerManagerDbContext : DbContext, IGroomerManagerDbContext
{
    private readonly IDateTime _dateTime;
    
    public GroomerManagerDbContext(DbContextOptions<GroomerManagerDbContext> options, IDateTime dateTime)
        : base(options)
    {
        _dateTime = dateTime;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GroomerManagerDbContext).Assembly);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientNote> ClientNotes { get; set; }
    public DbSet<Salon> Salons { get; set; }
    public DbSet<UserSalon> UserSalons { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userEmail =  "System";

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = _dateTime.Now;
                    entry.Entity.CreatedBy = userEmail;
                    entry.Entity.StatusId = 1;
                    break;

                case EntityState.Modified:
                    entry.Entity.Modified = _dateTime.Now;
                    entry.Entity.ModifiedBy = userEmail;
                    break;

                case EntityState.Deleted:
                    entry.Entity.Modified = _dateTime.Now;
                    entry.Entity.Inactivated = _dateTime.Now;
                    entry.Entity.InactivatedBy = userEmail;
                    entry.Entity.StatusId = 0;
                    entry.State = EntityState.Modified;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
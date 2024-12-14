using GroomerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    DbSet<Role> Roles { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
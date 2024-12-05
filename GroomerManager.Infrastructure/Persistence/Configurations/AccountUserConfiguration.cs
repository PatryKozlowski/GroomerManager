using GroomerManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroomerManager.Infrastructure.Persistence.Configurations;

public class AccountUserConfiguration : IEntityTypeConfiguration<AccountUser>
{
    public void Configure(EntityTypeBuilder<AccountUser> builder)
    {
        builder.HasOne(p => p.Account)
            .WithMany(a => a.AccountUsers)
            .HasForeignKey(k => k.AccountId);

        builder.HasOne(p => p.User)
            .WithMany(a => a.AccountUsers)
            .HasForeignKey(k => k.UserId);
    }
}

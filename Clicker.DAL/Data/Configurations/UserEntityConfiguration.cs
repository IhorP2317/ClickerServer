using Clicker.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clicker.DAL.Data.Configurations;
public class UserEntityConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.HasMany(u => u.Referrals)
            .WithOne(u => u.Referrer)
            .HasForeignKey(u => u.ReferrerId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Property(u => u.Balance)
            .HasColumnType("decimal(18,3)");
    }
}
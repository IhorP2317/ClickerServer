using Clicker.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clicker.DAL.Data.Configurations;

public class UserOfferSubscriptionTaskEntityConfiguration: IEntityTypeConfiguration<UserOfferSubscriptionTask>
{
    public void Configure(EntityTypeBuilder<UserOfferSubscriptionTask> builder)
    {
        builder.HasKey(ut => new { ut.UserId, ut.TaskId });
        builder.HasOne(ut => ut.User)
            .WithMany(u => u.UserOfferTasks)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ut => ut.OfferTask)
            .WithMany(t => t.UserOfferTasks)
            .HasForeignKey(ut => ut.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
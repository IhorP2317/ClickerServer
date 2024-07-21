using Clicker.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clicker.DAL.Data.Configurations;

public class UserChannelSubscriptionTaskEntityConfiguration : IEntityTypeConfiguration<UserChannelSubscriptionTask>
{
    public void Configure(EntityTypeBuilder<UserChannelSubscriptionTask> builder)
    {
        builder.HasKey(ut => new { ut.UserId, ut.TaskId });

        builder.HasOne(uc => uc.ChannelSubscriptionTask)
            .WithMany(c => c.UserChannelSubscriptionTasks)
            .HasForeignKey(ut => ut.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ut => ut.User)
            .WithMany(t => t.UserChannelTasks)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
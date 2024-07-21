using Clicker.DAL.Models;

using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace Clicker.DAL.Data.Configurations;



public class ChannelSubscriptionBaseEntityConfiguration : BaseEntityConfiguration<ChannelSubscriptionTask>
{
    public override void Configure(EntityTypeBuilder<ChannelSubscriptionTask> builder)
    {
       base.Configure(builder);
    }
}


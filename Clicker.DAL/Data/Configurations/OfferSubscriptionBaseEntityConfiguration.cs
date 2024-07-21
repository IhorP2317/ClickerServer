using Clicker.DAL.Models;

using Task = System.Threading.Tasks.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Clicker.DAL.Data.Configurations;



public class OfferSubscriptionBaseEntityConfiguration : BaseEntityConfiguration<OfferSubscriptionTask>
{
    public override void Configure(EntityTypeBuilder<OfferSubscriptionTask> builder)
    {
       base.Configure(builder);
    }
}

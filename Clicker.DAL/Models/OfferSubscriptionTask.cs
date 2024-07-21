namespace Clicker.DAL.Models;

public class OfferSubscriptionTask : BaseEntity
{
    public string OfferUrl { get; set; } = null!;
    public ICollection<UserOfferSubscriptionTask> UserOfferTasks { get; set; } = null!; // Renamed
}

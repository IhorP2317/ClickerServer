namespace Clicker.DAL.Models;
public class UserOfferSubscriptionTask
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid TaskId { get; set; }
    public OfferSubscriptionTask OfferTask { get; set; } = null!; 
    public bool IsCompleted { get; set; } 
}

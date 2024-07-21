namespace Clicker.DAL.Models;

public class User : BaseEntity
{
 public string TelegramId { get; set; } = null!;
 public int Energy { get; set; }
 public decimal Balance { get; set; }
 public string Role { get; set; } = null!;
 public Guid? ReferrerId { get; set; }
 public User Referrer { get; set; } = null!;
 public ICollection<User> Referrals { get; set; } = null!;
 public ICollection<UserChannelSubscriptionTask> UserChannelTasks { get; set; } = null!; 
 public ICollection<UserOfferSubscriptionTask> UserOfferTasks { get; set; } = null!; 
}

namespace Clicker.DAL.Models;

public class UserChannelSubscriptionTask
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid TaskId { get; set; }
    public ChannelSubscriptionTask ChannelSubscriptionTask { get; set; } = null!; 
    public bool IsCompleted { get; set; } 
}
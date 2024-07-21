namespace Clicker.DAL.Models;

public class ChannelSubscriptionTask:BaseEntity
{
    public string ChannelId { get; set; } = null!;
    public ICollection<UserChannelSubscriptionTask> UserChannelSubscriptionTasks { get; set; } = null!;

}
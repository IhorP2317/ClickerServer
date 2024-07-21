namespace Clicker.Domain.Dto.Task;

public record UserChannelSubscriptionTaskResponseDto
{
    public Guid TaskId { get; init; }
    public Guid UserId { get; init; }

    public string ChannelId { get; init; } = string.Empty;
    public bool IsCompleted { get; init; }

    public UserChannelSubscriptionTaskResponseDto() { }

    public UserChannelSubscriptionTaskResponseDto(Guid taskId, Guid userId, string channelId, bool isCompleted)
    {
        TaskId = taskId;
        UserId = userId;
        ChannelId = channelId;
        IsCompleted = isCompleted;
    }
}
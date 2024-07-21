namespace Clicker.Domain.Dto.Task;

public record UserTasksResponseDto(ICollection<UserChannelSubscriptionTaskResponseDto> ChannelTasks, ICollection<UserOfferSubscriptionTaskResponseDto> OfferTasks  );
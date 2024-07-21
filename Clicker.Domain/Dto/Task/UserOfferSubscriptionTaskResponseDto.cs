namespace Clicker.Domain.Dto.Task;

public record UserOfferSubscriptionTaskResponseDto(Guid TaskId, Guid UserId, string OfferUrl, bool IsCompleted);
namespace Clicker.Domain.Dto;

public record UserSecurityResponseDto(Guid Id, string TelegramId, Guid? ReferrerId, string Role);

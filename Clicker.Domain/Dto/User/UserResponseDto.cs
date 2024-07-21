namespace Clicker.Domain.Dto;

public record UserResponseDto(
    Guid Id,
  string TelegramId ,
  int Energy ,
  decimal Balance,
    string Role);
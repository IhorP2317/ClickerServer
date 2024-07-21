namespace Clicker.BL.Abstractions;

public class ICurrentUserService
{
    string? AccessTokenRaw { get; }
    string? UserId { get; }
    string? TelegramId { get; }
    string? UserRole { get; }
}
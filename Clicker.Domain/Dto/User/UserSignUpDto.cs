using System.ComponentModel.DataAnnotations;

namespace Clicker.Domain.Dto;

public record UserSignUpDto(
    [Required(AllowEmptyStrings = false, ErrorMessage = "Telegram id is required!")]
     string TelegramId,
    Guid? ReferrerId )
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
    [PasswordPolicy]
    public string Password { get; set; } = null!;
}
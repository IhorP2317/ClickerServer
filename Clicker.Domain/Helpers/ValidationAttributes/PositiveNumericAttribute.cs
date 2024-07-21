using System.ComponentModel.DataAnnotations;
using System.Globalization;

public class PositiveNumericAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult("TelegramId is required and cannot be empty.");
        }

        if (!ulong.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
        {
            return new ValidationResult("TelegramId must be a positive numeric value.");
        }

        return ValidationResult.Success;
    }
}
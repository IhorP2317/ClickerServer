
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class PasswordPolicyAttribute : ValidationAttribute
{
    public PasswordPolicyAttribute()
    {
        ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter," +
                       " one lowercase letter, one digit, and one special character.";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return new ValidationResult("Password is required.");
        }

        var password = value.ToString();
        var isValid = Regex.IsMatch(password,
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");

        if (!isValid)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
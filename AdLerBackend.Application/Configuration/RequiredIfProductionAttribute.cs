using System.ComponentModel.DataAnnotations;

namespace AdLerBackend.Application.Configuration;

public class RequiredIfProductionAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        if (!isDevelopment && value == null)
            return new ValidationResult(
                $"The {validationContext.MemberName} field is required when ASPNETCORE_ENVIRONMENT is 'Production'.");

        return ValidationResult.Success;
    }
}
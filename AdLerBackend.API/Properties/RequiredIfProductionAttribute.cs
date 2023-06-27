using System.ComponentModel.DataAnnotations;

namespace AdLerBackend.API.Properties;

public class RequiredIfProductionAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var environmentProperty = validationContext.ObjectType.GetProperty("ASPNETCORE_ENVIRONMENT");
        if (environmentProperty == null)
            return new ValidationResult(
                $"Cannot find property named 'ASPNETCORE_ENVIRONMENT' in {validationContext.ObjectType.FullName}");

        var environment = (string) environmentProperty.GetValue(validationContext.ObjectInstance);

        if (environment == "Production" && value == null)
            return new ValidationResult(
                $"The {validationContext.MemberName} field is required when ASPNETCORE_ENVIRONMENT is 'Production'.");

        return ValidationResult.Success;
    }
}
using System.ComponentModel.DataAnnotations;
using System.Text;

// ReSharper disable InconsistentNaming
#pragma warning disable CS1591

namespace AdLerBackend.API.Properties;

// Use Annotations to validate the configuration object
public class BackendConfig
{
    [RequiredIfProduction] public string ASPNETCORE_ADLER_MOODLEURL { get; set; } = null!;

    // retquired and has to be either "Production" or "Development"
    [Required]
    [RegularExpression("Production|Development")]
    public string ASPNETCORE_ENVIRONMENT { get; set; } = null!;

    /// <summary>
    ///     Override ToString() to get a formatted string with all properties using reflection
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        // return a formatted string with all properties using reflection
        var sb = new StringBuilder();
        var properties = GetType().GetProperties();
        foreach (var property in properties) sb.AppendLine($"{property.Name}: {property.GetValue(this, null)}");

        return sb.ToString();
    }
}
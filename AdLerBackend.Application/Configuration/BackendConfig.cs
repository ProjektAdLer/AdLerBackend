using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Configuration;

// Nullability warnings are disabled because the configuration object will be validated using the Annotations
#pragma warning disable CS8618
#pragma warning disable CS1591

namespace AdLerBackend.Application.Configuration;

public class BackendConfig
{
    [Required]
    [RegularExpression("Production|Development")]
    [ConfigurationKeyName("ASPNETCORE_ENVIRONMENT")]
    public string Environment { get; set; }

    [Required]
    [ConfigurationKeyName("ASPNETCORE_ADLER_MOODLEURL")]
    public string MoodleUrl { get; set; }

    [RequiredIfProduction]
    [ConfigurationKeyName("ASPNETCORE_DBPASSWORD")]
    public string DbPassword { get; set; }

    [RequiredIfProduction]
    [ConfigurationKeyName("ASPNETCORE_DBUSER")]
    public string DbUser { get; set; }

    [RequiredIfProduction]
    [ConfigurationKeyName("ASPNETCORE_DBNAME")]
    public string DbName { get; set; }

    [RequiredIfProduction]
    [ConfigurationKeyName("ASPNETCORE_DBHOST")]
    public string DbHost { get; set; }

    [RequiredIfProduction]
    [ConfigurationKeyName("ASPNETCORE_DBPORT")]
    public string DbPort { get; set; }

    // Not Required
    [ConfigurationKeyName("ASPNETCORE_ADLER_HTTPPORT")]
    public int HttpPort { get; set; } = 80;


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
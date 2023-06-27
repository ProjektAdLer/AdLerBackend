using System.Text;

namespace AdLerBackend.API.Properties;

// Use Annotations to validate the configuration object
public class BackendConfig
{
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
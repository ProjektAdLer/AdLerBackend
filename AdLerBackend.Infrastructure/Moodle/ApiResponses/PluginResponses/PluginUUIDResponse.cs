using System.Text.Json.Serialization;

namespace AdLerBackend.Infrastructure.Moodle.ApiResponses.PluginResponses;

public class PluginUUIDResponse
{
    [JsonPropertyName("course_id")] public string CourseId { get; set; }

    [JsonPropertyName("element_type")] public string ElementType { get; set; }

    [JsonPropertyName("Uuid")] public Guid Uuid { get; set; }

    [JsonPropertyName("moodle_id")] public int MoodleId { get; set; }

    [JsonPropertyName("context_id")] public int ContextId { get; set; }
}
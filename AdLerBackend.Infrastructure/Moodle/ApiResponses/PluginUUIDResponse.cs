namespace AdLerBackend.Infrastructure.Moodle.ApiResponses;

public class PluginUUIDResponse
{
    public string CourseId { get; set; }
    public string ElementType { get; set; }
    public string Uuid { get; set; }
    public int MoodleId { get; set; }
    public int ContextId { get; set; }
}
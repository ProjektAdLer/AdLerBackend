namespace AdLerBackend.Infrastructure.Moodle.ApiResponses;

public class PluginTasksResponse
{
    public IList<MoodleTaskResponse> Tasks { get; set; }
}

public class MoodleTaskResponse
{
    public string Uuid { get; set; }
    public string Status { get; set; }
}
namespace AdLerBackend.Infrastructure.Moodle.ApiResponses;

public class PluginTasksResponse
{
    public IList<MoodleTask> Tasks { get; set; }
}

public class MoodleTask
{
    public string uuid { get; set; }
    public string status { get; set; }
}
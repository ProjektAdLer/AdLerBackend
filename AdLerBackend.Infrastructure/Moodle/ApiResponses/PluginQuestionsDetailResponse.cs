namespace AdLerBackend.Infrastructure.Moodle.ApiResponses;

public class PluginQuestionsDetailResponse
{
    public List<PluginAdaptivityQuestion> Questions { get; set; }

    public class PluginAdaptivityQuestion
    {
        public string Uuid { get; set; }

        public string Status { get; set; }
        public string? Answers { get; set; }
    }
}
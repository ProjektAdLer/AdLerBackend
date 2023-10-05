using JetBrains.Annotations;

namespace AdLerBackend.Infrastructure.Moodle.ApiResponses;

[UsedImplicitly]
public class PluginQuestionsDetailResponse
{
    public List<PluginAdaptivityQuestion> Questions { get; set; }

    [UsedImplicitly]
    public class PluginAdaptivityQuestion
    {
        public string Uuid { get; set; }

        public string Status { get; set; }
        public string? Answers { get; set; }
    }
}
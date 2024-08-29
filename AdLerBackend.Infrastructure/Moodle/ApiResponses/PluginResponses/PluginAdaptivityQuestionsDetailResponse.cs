using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace AdLerBackend.Infrastructure.Moodle.ApiResponses.PluginResponses;

[UsedImplicitly]
public class PluginAdaptivityQuestionsDetailResponse
{
    public List<PluginAdaptivityQuestion> Questions { get; set; }

    [UsedImplicitly]
    public class PluginAdaptivityQuestion
    {
        public string Uuid { get; set; }

        public string Status { get; set; }

        [JsonPropertyName("status_best_try")] public string StatusBestTry { get; set; }

        public string? Answers { get; set; }
    }
}
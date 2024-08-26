namespace AdLerBackend.Infrastructure.Moodle.ApiResponses.PluginResponses;

public class PluginAdaptivityAnsweredQuestionResponse
{
    public AdaptivityModuleInfo Module { get; set; }
    public IList<PluginAdaptivityTaskResponse> Tasks { get; set; }
    public IList<PluginAdaptivityQuestionsDetailResponse.PluginAdaptivityQuestion> Questions { get; set; }

    public class AdaptivityModuleInfo
    { 
        public string Status { get; set; }
    }
}
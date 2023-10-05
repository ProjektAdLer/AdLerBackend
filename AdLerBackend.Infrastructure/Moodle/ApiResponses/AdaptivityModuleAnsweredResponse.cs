namespace AdLerBackend.Infrastructure.Moodle.ApiResponses;

public class AdaptivityModuleAnsweredResponse
{
    public AdaptivityModuleInfo Module { get; set; }
    public IList<MoodleTaskResponse> Tasks { get; set; }
    public IList<PluginQuestionsDetailResponse.PluginAdaptivityQuestion> Questions { get; set; }

    public class AdaptivityModuleInfo
    {
        public string Module_Id { get; set; }
        public string Instance_Id { get; set; }
        public string Status { get; set; }
    }
}
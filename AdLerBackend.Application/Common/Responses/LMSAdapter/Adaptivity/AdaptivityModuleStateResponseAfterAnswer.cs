namespace AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;

public class AdaptivityModuleStateResponseAfterAnswer
{
    public IEnumerable<LMSAdaptivityQuestionStateResponse> Questions { get; init; }
    public IEnumerable<LMSAdaptivityTaskStateResponse> Tasks { get; init; }
    public AdaptivityStates State { get; init; } = AdaptivityStates.NotAttempted;
}
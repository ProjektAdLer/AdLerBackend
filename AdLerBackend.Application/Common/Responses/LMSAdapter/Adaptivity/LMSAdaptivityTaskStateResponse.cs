namespace AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;

public class LMSAdaptivityTaskStateResponse
{
    public Guid Uuid { get; init; }
    public AdaptivityStates State { get; init; } = AdaptivityStates.notAttempted;
}
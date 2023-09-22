namespace AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;

public record LMSAdaptivityQuestionStateResponse
{
    public Guid Uuid { get; init; }

    public AdaptivityStates Status { get; init; } = AdaptivityStates.notAttempted;
}
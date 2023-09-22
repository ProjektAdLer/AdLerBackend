namespace AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;

public record LMSAdaptivityQuestionStateResponse
{
    public string Uuid { get; init; }

    // TODO: Change to Type according to new Plugin Mock
    public string State { get; init; }
}
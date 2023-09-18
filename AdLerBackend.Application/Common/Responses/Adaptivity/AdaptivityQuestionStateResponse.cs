namespace AdLerBackend.Application.Common.Responses.Adaptivity;

public record AdaptivityQuestionStateResponse
{
    public string Uuid { get; init; }

    // TODO: Change to Type according to new Plugin Mock
    public string State { get; init; }
}
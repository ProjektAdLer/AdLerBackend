using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.Elements;

namespace AdLerBackend.Application.Element.ScoreElement;

public record ScoreElementCommand : CommandWithToken<ScoreElementResponse>
{
    public int ElementId { get; init; }
    public int WorldId { get; init; }
    public ScoreElementParams? ScoreElementParams { get; init; }
}
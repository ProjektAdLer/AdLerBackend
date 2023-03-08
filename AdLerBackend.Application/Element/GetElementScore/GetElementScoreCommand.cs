using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Elements;

namespace AdLerBackend.Application.Element.GetElementScore;

public record GetElementScoreCommand : CommandWithToken<ElementScoreResponse>
{
    public int lerningWorldId { get; init; }
    public int ElementId { get; init; }
}
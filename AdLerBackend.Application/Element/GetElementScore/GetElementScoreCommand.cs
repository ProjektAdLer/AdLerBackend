using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Elements;

namespace AdLerBackend.Application.Element.GetElementScore;

/// <summary>
///     Command to get The Score of an Learning Element by its WorldID and ElementID
/// </summary>
public record GetElementScoreCommand : CommandWithToken<ElementScoreResponse>
{
    public int LearningWorldId { get; init; }
    public int ElementId { get; init; }
}
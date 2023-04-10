using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;

#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.ElementStrategies.GetElementScoreStrategies.
    GetH5PElementScoreStrategy;

public record GetH5PElementScoreStrategyCommand : CommandWithToken<ElementScoreResponse>
{
    public int ElementId { get; init; }
    public Modules ElementMoule { get; init; }
}
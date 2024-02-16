using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;

#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.
    ScoreGenericLearningElementStrategy;

public record ScoreGenericElementStrategyCommand : CommandWithToken<ScoreElementResponse>
{
    public LmsModule LmsModule { get; init; }
}
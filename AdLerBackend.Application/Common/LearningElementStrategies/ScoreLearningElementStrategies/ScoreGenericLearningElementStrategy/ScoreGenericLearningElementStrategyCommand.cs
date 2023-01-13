using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.
    ScoreGenericLearningElementStrategy;

public record ScoreGenericLearningElementStrategyCommand : CommandWithToken<ScoreLearningElementResponse>
{
    public Modules Module { get; init; }
}
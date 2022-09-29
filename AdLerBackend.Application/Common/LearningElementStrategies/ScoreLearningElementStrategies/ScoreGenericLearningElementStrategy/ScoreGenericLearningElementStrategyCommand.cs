using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.
    ScoreGenericLearningElementStrategy;

public record ScoreGenericLearningElementStrategyCommand : CommandWithToken<ScoreLearningElementResponse>
{
    public Modules Module { get; init; }
}
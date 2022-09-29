using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.ScoreH5PStrategy;

public record ScoreH5PElementStrategyCommand : CommandWithToken<ScoreLearningElementResponse>
{
    public Modules Module { get; init; }
    public ScoreElementParams ScoreElementParams { get; init; }
}
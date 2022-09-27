using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.Common.LearningElementStrategies.H5PLearningElementStrategy;

public record H5PLearningElementStrategyCommand : CommandWithToken<LearningElementScoreResponse>
{
    public int ElementId { get; init; }
    public Modules LearningElementMoule { get; init; }
}
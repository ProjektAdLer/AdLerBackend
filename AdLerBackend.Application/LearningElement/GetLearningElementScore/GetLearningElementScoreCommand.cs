using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.LearningElements;

namespace AdLerBackend.Application.LearningElement.GetLearningElementScore;

public record GetLearningElementScoreCommand : CommandWithToken<LearningElementScoreResponse>
{
    public int lerningWorldId { get; init; }
    public int learningElementId { get; init; }
}
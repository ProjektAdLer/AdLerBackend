using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.LearningElements;

namespace AdLerBackend.Application.LearningElement.ScoreLearningElement;

public record ScoreLearningElementCommand : CommandWithToken<ScoreLearningElementResponse>
{
    public int ElementId { get; init; }
    public int CourseId { get; init; }
    public ScoreElementParams? ScoreElementParams { get; init; }
}
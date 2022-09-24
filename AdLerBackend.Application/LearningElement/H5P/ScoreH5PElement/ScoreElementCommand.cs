using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.LearningElements;

namespace AdLerBackend.Application.LearningElement.H5P.ScoreH5PElement;

public record ScoreElementCommand : CommandWithToken<ScoreLearningElementResponse>
{
    public int ElementId { get; init; }
    public int CourseId { get; init; }
    public ScoreElementParams ScoreElementParams { get; init; }
}
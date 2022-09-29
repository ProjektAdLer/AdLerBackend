using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.LearningElements;

namespace AdLerBackend.Application.LearningElement.GetLearningElementSource;

public record GetLearningElementSourceCommand : CommandWithToken<GetLearningElementSourceResponse>
{
    public int ElementId { get; init; }
    public int CourseId { get; init; }
}
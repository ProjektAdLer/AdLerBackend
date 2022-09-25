using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Course.GetLearningElementStatus;

public record GetLearningElementStatusCommand : CommandWithToken<LearningElementStatusResponse>
{
    public int CourseId { get; init; }
}
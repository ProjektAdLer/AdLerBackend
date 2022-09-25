using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Common.InternalUseCases.GetAllLearningElementsFromLms;

public record GetAllLearningElementsFromLmsCommand : CommandWithToken<GetAllLearningElementsFromLmsResponse>
{
    public int CourseId { get; set; }
}
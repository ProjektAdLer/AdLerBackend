#pragma warning disable CS8618
using AdLerBackend.Application.Common.Responses.LearningElements;

namespace AdLerBackend.Application.Common.Responses.Course;

public class LearningElementStatusResponse
{
    public int courseId { get; set; }
    public IList<LearningElementScoreResponse> LearningElements { get; set; }
}
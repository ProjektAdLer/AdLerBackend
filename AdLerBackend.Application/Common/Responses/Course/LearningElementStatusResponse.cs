namespace AdLerBackend.Application.Common.Responses.Course;

public class LearningElementStatusResponse
{
    public int courseId { get; set; }
    public IList<LearningElementStatus> LearningElements { get; set; }
}

public class LearningElementStatus
{
    public int ElementId { get; set; }
    public bool IsSuccess { get; set; }
}
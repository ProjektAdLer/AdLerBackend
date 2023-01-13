#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.Responses.LMSAdapter;

public class MoodleCourseListResponse
{
    public int Total { get; set; }
    public List<MoodleCourse> Courses { get; set; }
}

public class MoodleCourse
{
    public int Id { get; set; }
    public string Fullname { get; set; }
}
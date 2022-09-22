namespace AdLerBackend.Application.Common.Responses.LMSAdapter;

public class CourseContent
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CourseContentResponse
{
    public List<CourseContent> Courses { get; set; }
}
#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.DTOs.Storage;

public class CourseDeleteDto
{
    public int AuthorId { get; set; }
    public string CourseName { get; set; }
}
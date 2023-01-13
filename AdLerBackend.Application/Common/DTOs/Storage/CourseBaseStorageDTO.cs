#pragma warning disable CS8618
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.Common.DTOs.Storage;

public class CourseBaseStorageDto
{
    public LearningWorldDtoResponse CourseInforamtion { get; set; }
    public int AuthorId { get; set; }
}
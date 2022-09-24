using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.LearningElements;

namespace AdLerBackend.Application.LearningElement.H5P.GetH5PFilePath;

public record GetH5PFilePathCommand : CommandWithToken<GetH5PFilePathResponse>
{
    public int ElementId { get; init; }
    public int CourseId { get; init; }
}
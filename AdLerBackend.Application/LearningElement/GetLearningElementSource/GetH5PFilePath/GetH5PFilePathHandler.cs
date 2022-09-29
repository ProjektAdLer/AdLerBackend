using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.LearningElement.GetLearningElementSource.GetH5PFilePath;

public class GetH5PFilePathHandler : IRequestHandler<GetH5PFilePathCommand, GetLearningElementSourceResponse>
{
    private readonly ICourseRepository _courseRepository;

    public GetH5PFilePathHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<GetLearningElementSourceResponse> Handle(GetH5PFilePathCommand request,
        CancellationToken cancellationToken)
    {
        // No Auth is needed here, because the file is public
        var course = await _courseRepository.GetAsync(request.CourseId);

        if (course == null) throw new NotFoundException("Course not Found");

        var h5PPath = course.H5PFilesInCourse.FirstOrDefault(x => x.ElementId == request.ElementId)?.Path;

        if (h5PPath == null) throw new NotFoundException("H5P File not Found");

        return new GetLearningElementSourceResponse
        {
            FilePath = h5PPath
        };
    }
}
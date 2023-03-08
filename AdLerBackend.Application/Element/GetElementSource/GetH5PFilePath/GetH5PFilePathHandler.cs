using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Elements;
using MediatR;

namespace AdLerBackend.Application.Element.GetElementSource.GetH5PFilePath;

public class GetH5PFilePathHandler : IRequestHandler<GetH5PFilePathCommand, GetElementSourceResponse>
{
    private readonly IWorldRepository _worldRepository;

    public GetH5PFilePathHandler(IWorldRepository worldRepository)
    {
        _worldRepository = worldRepository;
    }

    public async Task<GetElementSourceResponse> Handle(GetH5PFilePathCommand request,
        CancellationToken cancellationToken)
    {
        // No Auth is needed here, because the file is public
        var course = await _worldRepository.GetAsync(request.WorldId);

        if (course == null) throw new NotFoundException("Course not Found");

        var h5PPath = course.H5PFilesInCourse.FirstOrDefault(x => x.ElementId == request.ElementId)?.Path;

        if (h5PPath == null) throw new NotFoundException("H5P File not Found");

        return new GetElementSourceResponse
        {
            FilePath = h5PPath
        };
    }
}
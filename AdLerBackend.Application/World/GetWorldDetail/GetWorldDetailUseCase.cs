using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;

namespace AdLerBackend.Application.World.GetWorldDetail;

public class GetWorldDetailUseCase(
    IWorldRepository worldRepository,
    IFileAccess fileAccess,
    ISerialization serialization)
    : IRequestHandler<GetWorldDetailCommand, WorldAtfResponse>
{
    private readonly IFileAccess _fileAccess = fileAccess;

    /// <summary>
    ///     Get the course detail for a given course id
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotFoundException">Throws, if a course is not found either on the disc, the database or the moodle api</exception>
    public async Task<WorldAtfResponse> Handle(GetWorldDetailCommand request,
        CancellationToken cancellationToken)
    {
        // Get Course from Database
        var course = await worldRepository.GetAsync(request.WorldId);
        if (course == null)
            throw new NotFoundException("Course with the Id " + request.WorldId + " not found");

        return serialization.GetObjectFromJsonString<WorldAtfResponse>(course.AtfJson);
    }
}
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;

namespace AdLerBackend.Application.World.GetWorldDetail;

public class GetWorldDetailUseCase : IRequestHandler<GetWorldDetailCommand, WorldAtfResponse>
{
    private readonly IFileAccess _fileAccess;
    private readonly ISerialization _serialization;
    private readonly IWorldRepository _worldRepository;

    public GetWorldDetailUseCase(IWorldRepository worldRepository, IFileAccess fileAccess,
        ISerialization serialization)
    {
        _worldRepository = worldRepository;
        _fileAccess = fileAccess;
        _serialization = serialization;
    }

    /// <summary>
    ///     Get the course detail for a given course id
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotFoundException">Throws, if a course is not found either on the disc, the database or the moodle api</exception>
    public async Task<WorldAtfResponse> Handle(GetWorldDetailCommand request,
        CancellationToken cancellationToken)
    {
        // Get Course from Database
        var course = await _worldRepository.GetAsync(request.WorldId);
        if (course == null)
            throw new NotFoundException("Course with the Id " + request.WorldId + " not found");

        // Get Course DSL 
        await using var fileStream = _fileAccess.GetReadFileStream(course.DslLocation);

        // Parse DSL File
        var dslFile = await _serialization.GetObjectFromJsonStreamAsync<WorldAtfResponse>(fileStream);

        return dslFile;
    }
}
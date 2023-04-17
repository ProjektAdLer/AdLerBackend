using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Course;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;

public class
    GetAllElementsFromLmsHandler : IRequestHandler<GetAllElementsFromLmsCommand,
        GetAllElementsFromLmsWithAdLerIdResponse>
{
    private readonly IFileAccess _fileAccess;
    private readonly ILMS _lms;
    private readonly ISerialization _serialization;
    private readonly IWorldRepository _worldRepository;

    public GetAllElementsFromLmsHandler(IWorldRepository worldRepository, IFileAccess fileAccess,
        ISerialization serialization, ILMS lms)
    {
        _worldRepository = worldRepository;
        _fileAccess = fileAccess;
        _serialization = serialization;
        _lms = lms;
    }

    public async Task<GetAllElementsFromLmsWithAdLerIdResponse> Handle(GetAllElementsFromLmsCommand request,
        CancellationToken cancellationToken)
    {
        // Get Course from DB
        var course = await _worldRepository.GetAsync(request.WorldId) ??
                     throw new NotFoundException($"Course with the Id {request.WorldId} not found");

        // Get ATF File
        await using var fileStream = _fileAccess.GetReadFileStream(course.DslLocation);
        var dslObject = await _serialization.GetObjectFromJsonStreamAsync<WorldDtoResponse>(fileStream);

        var atfIdWithFileName = dslObject.World.Elements.Select(x => x.ElementId)
            .Select(x => new
                {Id = x, FileName = dslObject.World.Elements.Find(e => e.ElementId == x)?.ElementName})
            .ToList();

        var searchedCourse = await _lms.SearchWorldsAsync(request.WebServiceToken, course.Name);
        var courseContent = await _lms.GetWorldContentAsync(request.WebServiceToken, searchedCourse.Courses[0].Id);

        // Get the LMS-Modules by their name
        var modulesWithId = atfIdWithFileName.Select(x =>
        {
            var module = courseContent.SelectMany(c => c.Modules)
                .FirstOrDefault(m => m.Name == x.FileName);

            if (module == null)
                throw new NotFoundException($"Element with the Id {x.Id} not found");

            return new ModuleWithId {AdLerId = x.Id!, LmsModule = module};
        }).ToList();

        return new GetAllElementsFromLmsWithAdLerIdResponse
        {
            ModulesWithAdLerId = modulesWithId,
            LmsCourseId = searchedCourse.Courses[0].Id
        };
    }
}
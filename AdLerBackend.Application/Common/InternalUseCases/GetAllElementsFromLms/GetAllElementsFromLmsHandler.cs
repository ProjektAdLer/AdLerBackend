using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;

/// <summary>
///     Gets all Learning Elements from a LMS Course, that are referenced in the Course Description (DSL) from the AMG
/// </summary>
public class GetAllElementsFromLmsHandler : IRequestHandler<GetAllElementsFromLmsCommand,
    GetAllElementsFromLmsResponse>
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

    public async Task<GetAllElementsFromLmsResponse> Handle(GetAllElementsFromLmsCommand request,
        CancellationToken cancellationToken)
    {
        var data = new List<ModuleWIthIdAndFileName>();

        // Get Course from Database
        var course = await _worldRepository.GetAsync(request.WorldId);
        if (course == null)
            throw new NotFoundException("Course with the Id " + request.WorldId + " not found");

        // Get Course DSL 
        await using var fileStream = _fileAccess.GetReadFileStream(course.DslLocation);

        // Parse DSL File
        var dslObject = await _serialization.GetObjectFromJsonStreamAsync<WorldDtoResponse>(fileStream);

        dslObject.World.Elements.Select(x => x.ElementId).ToList().ForEach(x =>
        {
            data.Add(new ModuleWIthIdAndFileName
            {
                Id = x
            });
        });

        // Get Course from Moodle
        var searchedCourse = await _lms.SearchWorldsAsync(request.WebServiceToken, course.Name);

        // Get Course Content from Moodle
        var courseContent = await _lms.GetWorldContentAsync(request.WebServiceToken, searchedCourse.Courses[0].Id);

        foreach (var moduleWIthIdAndFileName in data)
        {
            moduleWIthIdAndFileName.FileName = dslObject.World.Elements
                                                   .Find(x => x.ElementId == moduleWIthIdAndFileName.Id)
                                                   ?.LmsElementIdentifier?.Value ??
                                               throw new NotFoundException("Element with the Id " +
                                                                           moduleWIthIdAndFileName.Id + " not found");

            moduleWIthIdAndFileName.Modules = courseContent.SelectMany(x => x.Modules)
                .FirstOrDefault(x => x.Name == moduleWIthIdAndFileName.FileName)!;
        }

        var response = new GetAllElementsFromLmsResponse
        {
            ModulesWithID = new List<ModuleWithId>()
        };

        foreach (var datapoint in data)
        {
            if (datapoint.Modules == null || datapoint.Id == null)
                throw new NotFoundException("Element with the Id " + datapoint.Id + " not found");
            response.ModulesWithID.Add(new ModuleWithId
            {
                Id = (int) datapoint.Id!,
                Module = datapoint.Modules
            });
        }

        return response;
    }

    private class ModuleWIthIdAndFileName
    {
        public string? FileName { get; set; }
        public int? Id { get; set; }
        public Modules? Modules { get; set; }
    }
}
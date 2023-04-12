using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.GetElementLmsInformation;

/// <summary>
///     Gets a single Element from Moodle which is specified int the DSL from AMG
/// </summary>
public class GetLearningElementLmsInformationHandler : IRequestHandler<GetElementLmsInformationCommand,
    GetElementLmsInformationResponse>
{
    private readonly IFileAccess _fileAccess;
    private readonly ILMS _ilms;
    private readonly ISerialization _serialization;
    private readonly IWorldRepository _worldRepository;

    public GetLearningElementLmsInformationHandler(ILMS ilms, IWorldRepository worldRepository,
        IFileAccess fileAccess, ISerialization serialization)
    {
        _ilms = ilms;
        _worldRepository = worldRepository;
        _fileAccess = fileAccess;
        _serialization = serialization;
    }


    public async Task<GetElementLmsInformationResponse> Handle(GetElementLmsInformationCommand request,
        CancellationToken cancellationToken)
    {
        // Get Course from Database
        var course = await _worldRepository.GetAsync(request.WorldId);
        if (course == null)
            throw new NotFoundException("Course with the Id " + request.WorldId + " not found");

        // Get Course DSL 
        await using var fileStream = _fileAccess.GetReadFileStream(course.DslLocation);

        // Parse DSL File
        var dslObject = await _serialization.GetObjectFromJsonStreamAsync<WorldDtoResponse>(fileStream);

        // Get Course from Moodle
        var searchedCourses = await _ilms.SearchWorldsAsync(request.WebServiceToken, course.Name);

        // Get Course Content from Moodle
        var courseContent = await _ilms.GetWorldContentAsync(request.WebServiceToken, searchedCourses.Courses[0].Id);


        var searchedFileName = dslObject.World.Elements.Find(x => x.ElementId == request.ElementId)?
            .LmsElementIdentifier
            .Value;

        if (searchedFileName == null)
            throw new NotFoundException("Element with the Id " + request.ElementId + " not found");


        var module = GetModuleFromCourse(courseContent, x => x.Name == searchedFileName) ??
                     throw new NotFoundException(
                         "Element with the Id " + request.ElementId + " not found");

        return new GetElementLmsInformationResponse
        {
            ElementData = module
        };
    }

    private static Modules? GetModuleFromCourse(IEnumerable<WorldContent> courseContent, Func<Modules, bool> del)
    {
        return courseContent.SelectMany(content => content.Modules).FirstOrDefault(del);
    }
}
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.GetAllLearningElementsFromLms;

/// <summary>
///     Gets all Learning Elements from a LMS Course, that are referenced in the Course Description (DSL) from the AMG
/// </summary>
public class GetAllLearningElementsFromLmsHandler : IRequestHandler<GetAllLearningElementsFromLmsCommand,
    GetAllLearningElementsFromLmsResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFileAccess _fileAccess;
    private readonly IMoodle _moodle;
    private readonly ISerialization _serialization;

    public GetAllLearningElementsFromLmsHandler(ICourseRepository courseRepository, IFileAccess fileAccess,
        ISerialization serialization, IMoodle moodle)
    {
        _courseRepository = courseRepository;
        _fileAccess = fileAccess;
        _serialization = serialization;
        _moodle = moodle;
    }

    public async Task<GetAllLearningElementsFromLmsResponse> Handle(GetAllLearningElementsFromLmsCommand request,
        CancellationToken cancellationToken)
    {
        var data = new List<ModuleWIthIdAndFileName>();

        // Get Course from Database
        var course = await _courseRepository.GetAsync(request.CourseId);
        if (course == null)
            throw new NotFoundException("Course with the Id " + request.CourseId + " not found");

        // Get Course DSL 
        await using var fileStream = _fileAccess.GetFileStream(course.DslLocation);

        // Parse DSL File
        var dslObject = await _serialization.GetObjectFromJsonStreamAsync<LearningWorldDtoResponse>(fileStream);

        dslObject.LearningWorld.LearningElements.Select(x => x.Id).ToList().ForEach(x =>
        {
            data.Add(new ModuleWIthIdAndFileName
            {
                Id = x
            });
        });

        // Get Course from Moodle
        var searchedCourse = await _moodle.SearchCoursesAsync(request.WebServiceToken, course.Name);

        // Get Course Content from Moodle
        var courseContent = await _moodle.GetCourseContentAsync(request.WebServiceToken, searchedCourse.Courses[0].Id);

        foreach (var moduleWIthIdAndFileName in data)
        {
            moduleWIthIdAndFileName.FileName = dslObject.LearningWorld.LearningElements
                                                   .Find(x => x.Id == moduleWIthIdAndFileName.Id)?.Identifier?.Value ??
                                               throw new NotFoundException("Element with the Id " +
                                                                           moduleWIthIdAndFileName.Id + " not found");

            moduleWIthIdAndFileName.Modules = courseContent.SelectMany(x => x.Modules)
                .FirstOrDefault(x => x.Name == moduleWIthIdAndFileName.FileName)!;
        }

        var response = new GetAllLearningElementsFromLmsResponse
        {
            ModulesWithID = new List<ModuleWithId>()
        };

        foreach (var datapoint in data)
            response.ModulesWithID.Add(new ModuleWithId
            {
                Id = (int) datapoint.Id!,
                Module = datapoint.Modules
            });

        return response;
    }

    private class ModuleWIthIdAndFileName
    {
        public string? FileName { get; set; }
        public int? Id { get; set; }
        public Modules? Modules { get; set; }
    }
}
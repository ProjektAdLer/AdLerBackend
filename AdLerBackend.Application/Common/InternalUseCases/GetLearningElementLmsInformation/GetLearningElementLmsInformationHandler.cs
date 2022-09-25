using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.GetLearningElementLmsInformation;

public class GetLearningElementLmsInformationHandler : IRequestHandler<GetLearningElementLmsInformationCommand,
    GetLearningElementLmsInformationResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFileAccess _fileAccess;
    private readonly IMoodle _moodle;
    private readonly ISerialization _serialization;

    public GetLearningElementLmsInformationHandler(IMoodle moodle, ICourseRepository courseRepository,
        IFileAccess fileAccess, ISerialization serialization)
    {
        _moodle = moodle;
        _courseRepository = courseRepository;
        _fileAccess = fileAccess;
        _serialization = serialization;
    }


    public async Task<GetLearningElementLmsInformationResponse> Handle(GetLearningElementLmsInformationCommand request,
        CancellationToken cancellationToken)
    {
        // Get Course from Database
        var course = await _courseRepository.GetAsync(request.CourseId);
        if (course == null)
            throw new NotFoundException("Course with the Id " + request.CourseId + " not found");

        // Get Course DSL 
        await using var fileStream = _fileAccess.GetFileStream(course.DslLocation);

        // Parse DSL File
        var dslObject = await _serialization.GetObjectFromJsonStreamAsync<LearningWorldDtoResponse>(fileStream);

        // Get Course from Moodle
        var searchedCourses = await _moodle.SearchCoursesAsync(request.WebServiceToken, course.Name);

        // Get Course Content from Moodle
        var courseContent = await _moodle.GetCourseContentAsync(request.WebServiceToken, searchedCourses.Courses[0].Id);


        var searchedFileName = dslObject.LearningWorld.LearningElements.Find(x => x.Id == request.ElementId)?
            .Identifier
            .Value;

        if (searchedFileName == null)
            throw new NotFoundException("Element with the Id " + request.ElementId + " not found");


        var module = GetModuleFromCourse(courseContent, x => x.Name == searchedFileName) ??
                     throw new NotFoundException(
                         "Element with the Id " + request.ElementId + " not found");

        return new GetLearningElementLmsInformationResponse
        {
            LearningElementData = module
        };
    }

    private static Modules? GetModuleFromCourse(CourseContent[] courseContent, Func<Modules, bool> del)
    {
        return courseContent.SelectMany(content => content.Modules).FirstOrDefault(del);
    }
}
using System.Text.Json;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.LearningElement.H5P.ScoreH5PElement;

public class ScoreH5PElementHandler : IRequestHandler<ScoreElementCommand, ScoreLearningElementResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFileAccess _fileAccess;
    private readonly IMoodle _moodle;
    private readonly ISerialization _serialization;

    public ScoreH5PElementHandler(ISerialization serialization, IMoodle moodle, ICourseRepository courseRepository,
        IFileAccess fileAccess)
    {
        _serialization = serialization;
        _moodle = moodle;
        _courseRepository = courseRepository;
        _fileAccess = fileAccess;
    }

    public async Task<ScoreLearningElementResponse> Handle(ScoreElementCommand request,
        CancellationToken cancellationToken)
    {
        // Get User Data
        var userData = await _moodle.GetMoodleUserDataAsync(request.WebServiceToken);

        // Get Course from Database
        var course = await _courseRepository.GetAsync(request.CourseId);
        if (course == null)
            throw new NotFoundException("Course with the Id " + request.CourseId + " not found");

        // Get Course DSL 
        await using var fileStream = _fileAccess.GetFileStream(course.DslLocation);

        // Parse DSL File
        var dslFile = await _serialization.GetObjectFromJsonStreamAsync<LearningWorldDtoResponse>(fileStream);

        // Get Course from Moodle
        var searchedCourses = await _moodle.SearchCoursesAsync(request.WebServiceToken, course.Name);

        // Get Course Content from Moodle
        var courseContent = await _moodle.GetCourseContentAsync(request.WebServiceToken, searchedCourses.Courses[0].Id);

        // Get Actual Context Id of H5P
        var searchedFileName = dslFile.LearningWorld.LearningElements.Find(x => x.Id == request.ElementId)?
            .Identifier
            .Value;

        if (searchedFileName == null)
            throw new NotFoundException("Element with the Id " + request.ElementId + " not found");

        int? contextId = null;

        foreach (var content in courseContent)
        {
            foreach (var contentModule in content.Modules)
            {
                if (contextId != null) break;
                if (contentModule.Name == searchedFileName)
                    contextId = contentModule.contextid;
            }

            if (contextId != null) break;
        }

        if (contextId is null)
            throw new NotFoundException("H5P File with the name " + searchedFileName +
                                        " not found on Moodle");

        // Deserialize the XAPI Event
        var xapiEvent =
            _serialization.GetObjectFromJsonString<RawH5PEvent>(request.ScoreElementParams.SerializedXapiEvent!);

        xapiEvent.actor.name = userData.MoodleUserName;
        xapiEvent.actor.mbox = userData.UserEmail;

        xapiEvent.@object.id = "https://testmoodle.cluuub.xyz/xapi/activity/" + contextId;

        // serialize the XAPI Event again
        var inText = JsonSerializer.Serialize(xapiEvent);

        // Send the XAPI Event to the LRS
        var isSuccess = await _moodle.ProcessXAPIStatementAsync(request.WebServiceToken, inText);

        return new ScoreLearningElementResponse
        {
            isSuceess = isSuccess
        };
    }
}
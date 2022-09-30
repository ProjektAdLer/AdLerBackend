using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Course;
using MediatR;

namespace AdLerBackend.Application.Course.GetCoursesForUser;

public class GetCoursesForUserHandler : IRequestHandler<GetCoursesForUserCommand, GetCourseOverviewResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMoodle _moodle;

    public GetCoursesForUserHandler(IMoodle moodle, ICourseRepository courseRepository)
    {
        _moodle = moodle;
        _courseRepository = courseRepository;
    }

    public async Task<GetCourseOverviewResponse> Handle(GetCoursesForUserCommand request,
        CancellationToken cancellationToken)
    {
        var coursesFromApi = await _moodle.GetCoursesForUserAsync(request.WebServiceToken);

        var courseStringList = coursesFromApi.Courses.Select(c => c.Fullname).ToList();

        var coursesFromDb =
            await _courseRepository.GetAllCoursesByStrings(courseStringList);

        return new GetCourseOverviewResponse
        {
            Courses = coursesFromDb.Select(c => new CourseResponse
            {
                CourseId = c.Id,
                CourseName = c.Name
            }).ToList()
        };
    }
}
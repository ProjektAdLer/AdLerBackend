using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using AdLerBackend.Application.Common.Responses.Course;
using MediatR;

namespace AdLerBackend.Application.Course.GetCoursesForAuthor;

public class GetCoursesForAuthorHandler : IRequestHandler<GetCoursesForAuthorCommand, GetCourseOverviewResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMediator _mediator;

    public GetCoursesForAuthorHandler(ICourseRepository courseRepository, IMediator mediator)
    {
        _courseRepository = courseRepository;
        _mediator = mediator;
    }

    public async Task<GetCourseOverviewResponse> Handle(GetCoursesForAuthorCommand request,
        CancellationToken cancellationToken)
    {
        // check if user is Admin
        await _mediator.Send(new CheckUserPrivilegesCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        var courses = await _courseRepository.GetAllCoursesForAuthor(request.AuthorId);

        return new GetCourseOverviewResponse
        {
            Courses = courses.Select(c => new CourseResponse
            {
                CourseId = c.Id,
                CourseName = c.Name
            }).ToList()
        };
    }
}
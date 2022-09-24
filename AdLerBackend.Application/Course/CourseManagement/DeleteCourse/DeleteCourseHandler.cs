using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using MediatR;

namespace AdLerBackend.Application.Course.CourseManagement.DeleteCourse;

public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, bool>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IFileAccess _fileAccess;
    private readonly IMediator _mediator;

    public DeleteCourseHandler(ICourseRepository courseRepository, IFileAccess fileAccess,
        IMediator mediator)
    {
        _courseRepository = courseRepository;
        _fileAccess = fileAccess;
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        // check if user is Admin
        await _mediator.Send(new CheckUserPrivilegesCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        // get course from db
        var course = await _courseRepository.GetAsync(request.CourseId);

        // Delete from file System
        _fileAccess.DeleteCourse(new CourseDeleteDto
        {
            AuthorId = course.AuthorId,
            CourseName = course.Name
        });

        // Delete from db
        await _courseRepository.DeleteAsync(request.CourseId);

        return true;
    }
}
using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.LMS.GetUserData;
using MediatR;

namespace AdLerBackend.Application.World.WorldManagement.DeleteWorld;

public class DeleteWorldUseCase(
    IWorldRepository worldRepository,
    IFileAccess fileAccess,
    IMediator mediator,
    ILMS lms)
    : IRequestHandler<DeleteWorldCommand, bool>
{
    public async Task<bool> Handle(DeleteWorldCommand request, CancellationToken cancellationToken)
    {
        var authorData = await mediator.Send(new GetLMSUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        // get course from db
        var course = await worldRepository.GetAsync(request.WorldId);

        if (course == null)
            throw new NotFoundException("Course With Id: " + request.WorldId + " Not Found");


        if (course.AuthorId != authorData.UserId)
            throw new UnauthorizedAccessException("The Course does not belong to the User");

        // Delete from Moodle
        await lms.DeleteCourseAsync(request.WebServiceToken, course.LmsWorldId);
        //Delete from FileSystem
        fileAccess.DeleteWorld(new WorldDeleteDto
        {
            WorldInstanceId = course.LmsWorldId
        });
        // Delete from DB
        await worldRepository.DeleteAsync(request.WorldId);

        return true;
    }
}
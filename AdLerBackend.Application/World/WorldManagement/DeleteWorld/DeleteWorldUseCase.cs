using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.LMS.GetUserData;
using MediatR;

namespace AdLerBackend.Application.World.WorldManagement.DeleteWorld;

public class DeleteWorldUseCase : IRequestHandler<DeleteWorldCommand, bool>
{
    private readonly IFileAccess _fileAccess;
    private readonly ILMS _lms;
    private readonly IMediator _mediator;
    private readonly IWorldRepository _worldRepository;

    public DeleteWorldUseCase(IWorldRepository worldRepository, IFileAccess fileAccess,
        IMediator mediator, ILMS lms)
    {
        _worldRepository = worldRepository;
        _fileAccess = fileAccess;
        _mediator = mediator;
        _lms = lms;
    }

    public async Task<bool> Handle(DeleteWorldCommand request, CancellationToken cancellationToken)
    {
        var authorData = await _mediator.Send(new GetLMSUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        // get course from db
        var course = await _worldRepository.GetAsync(request.WorldId);

        if (course == null)
            throw new NotFoundException("Course With Id: " + request.WorldId + " Not Found");


        if (course.AuthorId != authorData.UserId)
            throw new UnauthorizedAccessException("The Course does not belong to the User");

        // Delete from Moodle
        await _lms.DeleteCourseAsync(request.WebServiceToken, course.LmsWorldId);
        //Delete from FileSystem
        _fileAccess.DeleteWorld(new WorldDeleteDto
        {
            WorldInstanceId = course.LmsWorldId
        });
        // Delete from DB
        await _worldRepository.DeleteAsync(request.WorldId);

        return true;
    }
}
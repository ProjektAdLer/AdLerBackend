using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using AdLerBackend.Application.LMS.GetUserData;
using MediatR;

namespace AdLerBackend.Application.World.WorldManagement.DeleteWorld;

public class DeleteWorldHandler : IRequestHandler<DeleteWorldCommand, bool>
{
    private readonly IFileAccess _fileAccess;
    private readonly IMediator _mediator;
    private readonly IWorldRepository _worldRepository;

    public DeleteWorldHandler(IWorldRepository worldRepository, IFileAccess fileAccess,
        IMediator mediator)
    {
        _worldRepository = worldRepository;
        _fileAccess = fileAccess;
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteWorldCommand request, CancellationToken cancellationToken)
    {
        // check if user is Admin
        await _mediator.Send(new CheckUserPrivilegesCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

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


        // Delete from file System
        _fileAccess.DeleteWorld(new WorldDeleteDto
        {
            AuthorId = course.AuthorId,
            WorldName = course.Name
        });

        // Delete from db
        await _worldRepository.DeleteAsync(request.WorldId);

        return true;
    }
}
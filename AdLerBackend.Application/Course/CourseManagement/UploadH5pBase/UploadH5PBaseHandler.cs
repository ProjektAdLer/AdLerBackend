using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using MediatR;

namespace AdLerBackend.Application.Course.CourseManagement.UploadH5pBase;

public class UploadH5PBaseHandler : IRequestHandler<UploadH5PBaseCommand, bool>
{
    private readonly IFileAccess _fileAccess;
    private readonly IMediator _mediator;

    public UploadH5PBaseHandler(IFileAccess fileAccess, IMediator mediator)
    {
        _fileAccess = fileAccess;
        _mediator = mediator;
    }

    public async Task<bool> Handle(UploadH5PBaseCommand request, CancellationToken cancellationToken)
    {
        // Check, if user is Admin
        await _mediator.Send(new CheckUserPrivilegesCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        _fileAccess.StoreH5PBase(request.H5PBaseZipStream);

        return true;
    }
}
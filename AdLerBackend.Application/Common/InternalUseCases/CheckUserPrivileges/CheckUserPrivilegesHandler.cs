using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;

public class CheckUserPrivilegesHandler : IRequestHandler<CheckUserPrivilegesCommand, Unit>
{
    private readonly IMoodle _moodle;

    public CheckUserPrivilegesHandler(IMoodle moodle)
    {
        _moodle = moodle;
    }

    public async Task<Unit> Handle(CheckUserPrivilegesCommand request, CancellationToken cancellationToken)
    {
        if (!await _moodle.IsMoodleAdminAsync(request.WebServiceToken))
            throw new ForbiddenAccessException("User is not " + request.Role);

        return Unit.Value;
    }
}
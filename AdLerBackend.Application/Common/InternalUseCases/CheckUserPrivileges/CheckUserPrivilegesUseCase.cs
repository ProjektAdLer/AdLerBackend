using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;

public class CheckUserPrivilegesUseCase : IRequestHandler<CheckUserPrivilegesCommand, Unit>
{
    private readonly ILMS _lms;

    public CheckUserPrivilegesUseCase(ILMS lms)
    {
        _lms = lms;
    }

    public async Task<Unit> Handle(CheckUserPrivilegesCommand request, CancellationToken cancellationToken)
    {
        if (!await _lms.IsLMSAdminAsync(request.WebServiceToken))
            throw new ForbiddenAccessException("User is not " + request.Role);

        return Unit.Value;
    }
}
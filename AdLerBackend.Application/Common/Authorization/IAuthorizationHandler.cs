using MediatR;

namespace AdLerBackend.Application.Common.Authorization;

public interface IAuthorizationHandler<TRequest> : IRequestHandler<TRequest, AuthorizationResult>
    where TRequest : IRequest<AuthorizationResult>
{
}
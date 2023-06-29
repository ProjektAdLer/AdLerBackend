using MediatR;

namespace AdLerBackend.Application.Common.Authorization;

public interface IAuthorizationRequirement : IRequest<AuthorizationResult>
{
}
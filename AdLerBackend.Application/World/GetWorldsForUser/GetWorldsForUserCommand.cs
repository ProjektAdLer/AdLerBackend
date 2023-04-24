using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.World;

namespace AdLerBackend.Application.World.GetWorldsForUser;

public record GetWorldsForUserCommand : CommandWithToken<GetWorldOverviewResponse>
{
}
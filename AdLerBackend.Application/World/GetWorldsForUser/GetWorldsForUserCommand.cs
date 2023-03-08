using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.World.GetWorldsForUser;

public record GetWorldsForUserCommand : CommandWithToken<GetWorldOverviewResponse>
{
}
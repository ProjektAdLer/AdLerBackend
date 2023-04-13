using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Course;

namespace AdLerBackend.Application.World.GetElementStatus;

public record GetWorldStatusCommand : CommandWithToken<WorldStatusResponse>
{
    public int WorldId { get; init; }
}
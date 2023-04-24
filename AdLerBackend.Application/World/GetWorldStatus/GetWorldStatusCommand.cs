using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.World;

namespace AdLerBackend.Application.World.GetWorldStatus;

public record GetWorldStatusCommand : CommandWithToken<WorldStatusResponse>
{
    public int WorldId { get; init; }
}
using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.World;

namespace AdLerBackend.Application.World.GetWorldDetail;

public record GetWorldDetailCommand : CommandWithToken<WorldAtfResponse>
{
    public int WorldId { get; init; }
}
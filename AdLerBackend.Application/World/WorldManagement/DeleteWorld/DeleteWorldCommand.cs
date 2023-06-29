using AdLerBackend.Application.Common;

namespace AdLerBackend.Application.World.WorldManagement.DeleteWorld;

public record DeleteWorldCommand : CommandWithToken<bool>
{
    public int WorldId { get; init; }
}
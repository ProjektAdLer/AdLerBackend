using AdLerBackend.Application.Common;

namespace AdLerBackend.Application.Player.DeletePlayerData;

public record DeletePlayerDataCommand : CommandWithToken<bool>
{
}
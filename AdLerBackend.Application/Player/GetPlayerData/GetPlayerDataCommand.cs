using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Player;

namespace AdLerBackend.Application.Player.GetPlayerData;

public record GetPlayerDataCommand : CommandWithToken<PlayerDataResponse>;
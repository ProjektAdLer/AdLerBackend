using AdLerBackend.Domain.Entities.PlayerData;

namespace AdLerBackend.Application.Common.Responses.Player;

public class PlayerDataResponse
{
    public PlayerAvatarGender Gender { get; set; }
    public PlayerWorldColor WorldColor { get; set; }
}
using AdLerBackend.Domain.Entities.PlayerData;

namespace AdLerBackend.Application.Common.Responses.Player;

public class PlayerDataResponse
{
    public PlayerAvatarGender PlayerGender { get; set; }
    public PlayerWorldColor PlayerWorldColor { get; set; }
}
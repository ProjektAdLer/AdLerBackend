using AdLerBackend.Domain.Entities.Common;

#pragma warning disable CS8618
namespace AdLerBackend.Domain.Entities.PlayerData;

// All the data here has to have default values

public class PlayerData : BaseEntity
{
    public PlayerAvatarGender PlayerGender { get; set; } = PlayerAvatarGender.Male;
    public PlayerWorldColor PlayerWorldColor { get; set; } = PlayerWorldColor.Blue;
}
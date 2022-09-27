namespace AdLerBackend.Domain.Entities.PlayerData;

// All the data here has to have default values

public class PlayerData
{
    public PlayerAvatarGender PlayerGender { get; set; } =  PlayerAvatarGender.Male;
    public PlayerWorldColor PlayerWorldColor { get; set; } = PlayerWorldColor.Blue;
}
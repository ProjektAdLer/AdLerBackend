using AdLerBackend.Domain.Entities.PlayerData;

namespace AdLerBackend.Domain.UnitTests.TestingUtils;

public static class PlayerDataEntityFactory
{
    public static PlayerData CreatePlayerData(
        PlayerAvatarGender gender = PlayerAvatarGender.Male,
        PlayerWorldColor worldColor = PlayerWorldColor.Blue,
        int? id = null)
    {
        return new PlayerData(gender, worldColor, id);
    }
}
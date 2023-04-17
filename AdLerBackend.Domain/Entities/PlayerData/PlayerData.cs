using JetBrains.Annotations;

namespace AdLerBackend.Domain.Entities.PlayerData;

// All the data here has to have default values

public class PlayerData : IBaseEntity
{
    /// <summary>
    ///     This Empty Constructor is needed for EntityFramework as well as for AutoMapper.
    ///     see https://docs.microsoft.com/en-us/ef/core/modeling/constructors
    /// </summary>
    [UsedImplicitly]
    private PlayerData()
    {
        // initialize all properties with default values
        Id = null;
        PlayerGender = PlayerAvatarGender.Male;
        PlayerWorldColor = PlayerWorldColor.Blue;
    }

    public PlayerData(PlayerAvatarGender playerGender,
        PlayerWorldColor playerWorldColor, int? id = null)
    {
        PlayerGender = playerGender;
        PlayerWorldColor = playerWorldColor;
        Id = id;
    }

    public PlayerAvatarGender PlayerGender { get; set; }
    public PlayerWorldColor PlayerWorldColor { get; set; }
    public int? Id { get; set; }
}
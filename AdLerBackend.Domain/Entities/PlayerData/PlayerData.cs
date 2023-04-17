using JetBrains.Annotations;

namespace AdLerBackend.Domain.Entities.PlayerData;

// All the data here has to have default values

public class PlayerData : IBaseEntity
{
#pragma warning disable CS8618
    /// <summary>
    ///     This Empty Constructor is needed for EntityFramework
    ///     see https://docs.microsoft.com/en-us/ef/core/modeling/constructors
    /// </summary>
    [UsedImplicitly]
    public PlayerData()
    {
    }
#pragma warning restore CS8618
    public PlayerAvatarGender PlayerGender { get; set; } = PlayerAvatarGender.Male;
    public PlayerWorldColor PlayerWorldColor { get; set; } = PlayerWorldColor.Blue;
    public int? Id { get; init; }
}
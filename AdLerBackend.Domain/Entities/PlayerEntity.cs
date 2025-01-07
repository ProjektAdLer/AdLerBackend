namespace AdLerBackend.Domain.Entities;

public class PlayerEntity : IBaseEntity
{
    public int LmsId { get; set; }
    public string Username { get; set; }
    public AvatarEntity Avatar { get; set; }

    // Navigational Properties
    public int? AvatarId { get; set; }
    public AvatarEntity AvatarEntity { get; set; }
    public int? Id { get; set; }
}
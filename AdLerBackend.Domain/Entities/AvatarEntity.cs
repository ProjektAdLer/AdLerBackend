namespace AdLerBackend.Domain.Entities;

public class AvatarEntity : IBaseEntity
{
    // Face
    public string? Eyebrows { get; set; }
    public string? Eyes { get; set; }
    public string? Nose { get; set; }
    public string? Mouth { get; set; }

    // Hair
    public string? Hair { get; set; }
    public string? Beard { get; set; }
    public int? HairColor { get; set; }

    // Accessories  
    public string? Headgear { get; set; }
    public string? Glasses { get; set; }

    // Clothes
    public string? Shirt { get; set; }
    public int? ShirtColor { get; set; }
    public string? Pants { get; set; }
    public int? PantsColor { get; set; }
    public string? Shoes { get; set; }
    public int? ShoesColor { get; set; }

    // Body
    public string? SkinColor { get; set; }
    public float Roundness { get; set; }

    // Navigational Properties
    public required int PlayerDataId { get; set; }
    public PlayerEntity PlayerEntity { get; set; }
    public int? Id { get; set; }
}
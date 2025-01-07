using AdLerBackend.Domain.Entities;

namespace AdLerBackend.Application.Common.DTOs;

public class AvatarApiDto
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

    public static AvatarApiDto FromEntity(AvatarEntity avatar)
    {
        return new AvatarApiDto
        {
            Eyebrows = avatar.Eyebrows,
            Eyes = avatar.Eyes,
            Nose = avatar.Nose,
            Mouth = avatar.Mouth,
            Hair = avatar.Hair,
            Beard = avatar.Beard,
            HairColor = avatar.HairColor,
            Headgear = avatar.Headgear,
            Glasses = avatar.Glasses,
            Shirt = avatar.Shirt,
            ShirtColor = avatar.ShirtColor,
            Pants = avatar.Pants,
            PantsColor = avatar.PantsColor,
            Shoes = avatar.Shoes,
            ShoesColor = avatar.ShoesColor,
            SkinColor = avatar.SkinColor,
            Roundness = avatar.Roundness
        };
    }

    public static AvatarEntity ToEntity(AvatarApiDto avatar, int playerId)
    {
        return new AvatarEntity
        {
            PlayerDataId = playerId,
            Eyebrows = avatar.Eyebrows,
            Eyes = avatar.Eyes,
            Nose = avatar.Nose,
            Mouth = avatar.Mouth,
            Hair = avatar.Hair,
            Beard = avatar.Beard,
            HairColor = avatar.HairColor,
            Headgear = avatar.Headgear,
            Glasses = avatar.Glasses,
            Shirt = avatar.Shirt,
            ShirtColor = avatar.ShirtColor,
            Pants = avatar.Pants,
            PantsColor = avatar.PantsColor,
            Shoes = avatar.Shoes,
            ShoesColor = avatar.ShoesColor,
            SkinColor = avatar.SkinColor,
            Roundness = avatar.Roundness
        };
    }

    public static void UpdateEntity(AvatarEntity avatar, AvatarApiDto avatarData)
    {
        avatar.Eyebrows = avatarData.Eyebrows;
        avatar.Eyes = avatarData.Eyes;
        avatar.Nose = avatarData.Nose;
        avatar.Mouth = avatarData.Mouth;
        avatar.Hair = avatarData.Hair;
        avatar.Beard = avatarData.Beard;
        avatar.HairColor = avatarData.HairColor;
        avatar.Headgear = avatarData.Headgear;
        avatar.Glasses = avatarData.Glasses;
        avatar.Shirt = avatarData.Shirt;
        avatar.ShirtColor = avatarData.ShirtColor;
        avatar.Pants = avatarData.Pants;
        avatar.PantsColor = avatarData.PantsColor;
        avatar.Shoes = avatarData.Shoes;
        avatar.ShoesColor = avatarData.ShoesColor;
        avatar.SkinColor = avatarData.SkinColor;
        avatar.Roundness = avatarData.Roundness;
    }
}
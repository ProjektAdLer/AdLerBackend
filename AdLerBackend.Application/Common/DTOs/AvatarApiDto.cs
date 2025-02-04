using System.Diagnostics.CodeAnalysis;
using AdLerBackend.Domain.Entities;

namespace AdLerBackend.Application.Common.DTOs;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class AvatarApiDto
{
    // Face
    public string? Eyebrows { get; init; }
    public string? Eyes { get; init; }
    public string? Nose { get; init; }
    public string? Mouth { get; init; }

    // Hair
    public string? Hair { get; init; }
    public string? Beard { get; init; }
    public int? HairColor { get; init; }

    // Accessories  
    public string? Headgear { get; init; }
    public string? Glasses { get; init; }
    public string? Backpack { get; init; }
    public string? Other { get; init; }

    // Clothes
    public string? Shirt { get; init; }
    public int? ShirtColor { get; init; }
    public string? Pants { get; init; }
    public int? PantsColor { get; init; }
    public string? Shoes { get; init; }
    public int? ShoesColor { get; init; }

    // Body
    public int? SkinColor { get; init; }
    public float Roundness { get; init; }

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
            Roundness = avatar.Roundness,
            Backpack = avatar.Backpack,
            Other = avatar.Other
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
            Roundness = avatar.Roundness,
            Backpack = avatar.Backpack,
            Other = avatar.Other
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
        avatar.Backpack = avatarData.Backpack;
        avatar.Other = avatarData.Other;
    }
}
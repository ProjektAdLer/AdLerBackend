using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.DTOs;

namespace AdLerBackend.Application.Avatar.SetPlayerAvatar;

public record SetPlayerAvatarCommand : CommandWithToken<AvatarApiDto>
{
    public int PlayerId { get; init; }
    public AvatarApiDto AvatarData { get; init; }
}
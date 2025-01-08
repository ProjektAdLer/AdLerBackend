using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.DTOs;

namespace AdLerBackend.Application.Avatar.SetPlayerAvatar;

public record SetPlayerAvatarCommand : CommandWithToken<AvatarApiDto>
{
    public AvatarApiDto AvatarData { get; init; }
}
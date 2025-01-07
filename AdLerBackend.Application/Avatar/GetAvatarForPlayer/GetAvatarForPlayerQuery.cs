using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.DTOs;

namespace AdLerBackend.Application.Avatar.GetAvatarForPlayer;

public record GetAvatarForPlayerQuery : CommandWithToken<AvatarApiDto?>
{
    public int PlayerId { get; init; }
}
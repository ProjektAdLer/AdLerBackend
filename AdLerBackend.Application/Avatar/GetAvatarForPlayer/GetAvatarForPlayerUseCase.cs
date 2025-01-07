using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.LMS.GetUserData;
using MediatR;

namespace AdLerBackend.Application.Avatar.GetAvatarForPlayer;

public class GetAvatarForPlayerUseCase(
    IMediator mediator,
    IPlayerRepository playerRepository,
    IAvatarRepository avatarRepository)
    : IRequestHandler<GetAvatarForPlayerQuery, AvatarApiDto?>
{
    public async Task<AvatarApiDto?> Handle(GetAvatarForPlayerQuery request, CancellationToken cancellationToken)
    {
        var playerData = await mediator.Send(new GetLMSUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        });

        if (request.PlayerId != playerData.UserId)
            throw new ForbiddenAccessException("The Token provides does not belong to the player requested");

        var avatarData = await avatarRepository.GetAvatarForPlayerAsync(request.PlayerId);

        return avatarData == null ? null : AvatarApiDto.FromEntity(avatarData);
    }
}
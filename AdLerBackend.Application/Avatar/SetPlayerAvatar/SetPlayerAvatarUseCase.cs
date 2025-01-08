using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.LMS.GetUserData;
using MediatR;

namespace AdLerBackend.Application.Avatar.SetPlayerAvatar;

public class SetPlayerAvatarUseCase(
    IMediator mediator,
    IPlayerRepository playerRepository,
    IAvatarRepository avatarRepository) : IRequestHandler<SetPlayerAvatarCommand, AvatarApiDto>
{
    public async Task<AvatarApiDto> Handle(SetPlayerAvatarCommand request, CancellationToken cancellationToken)
    {
        var playerData = await mediator.Send(new GetLMSUserDataCommand
        {
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        var avatarData = await avatarRepository.GetAvatarForPlayerAsync(playerData.UserId);

        if (avatarData == null)
        {
            var avatar = AvatarApiDto.ToEntity(request.AvatarData, playerData.UserId);
            await avatarRepository.AddAsync(avatar);
            return AvatarApiDto.FromEntity(avatar);
        }

        AvatarApiDto.UpdateEntity(avatarData, request.AvatarData);
        await avatarRepository.UpdateAsync(avatarData);
        return AvatarApiDto.FromEntity(avatarData);
    }
}
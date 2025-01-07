using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
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

        if (request.PlayerId != playerData.UserId)
            throw new ForbiddenAccessException("The Token provides does not belong to the player requested");

        var avatarData = await avatarRepository.GetAvatarForPlayerAsync(request.PlayerId);

        if (avatarData == null)
        {
            var avatar = AvatarApiDto.ToEntity(request.AvatarData, request.PlayerId);
            await avatarRepository.AddAsync(avatar);
            return AvatarApiDto.FromEntity(avatar);
        }

        AvatarApiDto.UpdateEntity(avatarData, request.AvatarData);
        await avatarRepository.UpdateAsync(avatarData);
        return AvatarApiDto.FromEntity(avatarData);
    }
}
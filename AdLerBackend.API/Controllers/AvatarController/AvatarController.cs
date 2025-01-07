using AdLerBackend.Application.Avatar.GetAvatarForPlayer;
using AdLerBackend.Application.Avatar.SetPlayerAvatar;
using AdLerBackend.Application.Common.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.API.Controllers.AvatarController;

[Route("api/Users")]
public class AvatarController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("{playerId}/Avatar")]
    public async Task<AvatarApiDto?> GetAvatar([FromHeader] string token, [FromRoute] int playerId)
    {
        return await Mediator.Send(new GetAvatarForPlayerQuery
        {
            WebServiceToken = token,
            PlayerId = playerId
        });
    }

    /**
     * NOTE: POST both creates and updates the resource. This adhears to the REST principles.
     * It is not Restful, however.
     */
    [HttpPost("{playerId}/Avatar")]
    public async Task<AvatarApiDto> PostAvatar([FromHeader] string token, [FromRoute] int playerId,
        [FromBody] AvatarApiDto avatar)
    {
        return await Mediator.Send(new SetPlayerAvatarCommand
        {
            WebServiceToken = token,
            PlayerId = playerId,
            AvatarData = avatar
        });
    }
}
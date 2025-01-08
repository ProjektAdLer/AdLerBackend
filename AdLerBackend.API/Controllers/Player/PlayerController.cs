using AdLerBackend.Application.Avatar.GetAvatarForPlayer;
using AdLerBackend.Application.Avatar.SetPlayerAvatar;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LMS.GetUserData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.API.Controllers.Player;

[Route("api/Player/")]
public class PlayerController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<LMSUserDataResponse>> GetLmsUserData(
        [FromHeader] string token)
    {
        return await Mediator.Send(new GetLMSUserDataCommand
        {
            WebServiceToken = token
        });
    }

    [HttpGet("Avatar")]
    public async Task<AvatarApiDto?> GetAvatar([FromHeader] string token)
    {
        return await Mediator.Send(new GetAvatarForPlayerQuery
        {
            WebServiceToken = token
        });
    }

    /**
     * NOTE: POST both creates and updates the resource. This adhears to the REST principles.
     * It is not Restful, however.
     */
    [HttpPost("Avatar")]
    public async Task<AvatarApiDto> PostAvatar([FromHeader] string token,
        [FromBody] AvatarApiDto avatar)
    {
        return await Mediator.Send(new SetPlayerAvatarCommand
        {
            WebServiceToken = token,
            AvatarData = avatar
        });
    }
}
using AdLerBackend.Application.Common.Responses.Player;
using AdLerBackend.Application.Player.GetPlayerData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.API.Controllers.Player;

public class PlayerController : BaseApiController
{
    public PlayerController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<ActionResult<PlayerDataResponse>> GetPlayer([FromHeader] string token)
    {
        return await Mediator.Send(new GetPlayerDataCommand
        {
            WebServiceToken = token
        });
    }
}
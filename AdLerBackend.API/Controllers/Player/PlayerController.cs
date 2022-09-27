using AdLerBackend.Application.Common.Responses.Player;
using AdLerBackend.Application.Player.DeletePlayerData;
using AdLerBackend.Application.Player.GetPlayerData;
using AdLerBackend.Application.Player.UpdatePlayerData;
using AdLerBackend.Domain.Entities.PlayerData;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
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

    [HttpPatch]
    public async Task<ActionResult<PlayerDataResponse>> UpdatePlayer([FromHeader] string token,
        [FromBody] JsonPatchDocument<PlayerData> patch)
    {
        return await Mediator.Send(new UpdatePlayerCommand
        {
            PatchDocument = patch,
            WebServiceToken = token
        });
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> DeletePlayer([FromHeader] string token)
    {
        return await Mediator.Send(new DeletePlayerDataCommand
        {
            WebServiceToken = token
        });
    }
}
using AdLerBackend.Application.Common.Responses.Player;
using AdLerBackend.Application.Player.DeletePlayerData;
using AdLerBackend.Application.Player.GetPlayerData;
using AdLerBackend.Application.Player.UpdatePlayerData;
using AdLerBackend.Domain.Entities.PlayerData;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.API.Controllers.Player;

/// <summary>
///     Player controller
/// </summary>
[Route("api/PlayerData")]
public class PlayerController : BaseApiController
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="mediator"></param>
    public PlayerController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    ///     Get player data
    /// </summary>
    /// <param name="token">LMS Token</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<PlayerDataResponse>> GetPlayer([FromHeader] string token)
    {
        return await Mediator.Send(new GetPlayerDataCommand
        {
            WebServiceToken = token
        });
    }


    /// <summary>
    ///     Update player data
    /// </summary>
    /// <param name="token">LMS Token</param>
    /// <param name="patch">The Value to update in JsonPatch Notation</param>
    /// <returns></returns>
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

    /// <summary>
    ///     Delete player data
    /// </summary>
    /// <param name="token">LMS Token</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<ActionResult<bool>> DeletePlayer([FromHeader] string token)
    {
        return await Mediator.Send(new DeletePlayerDataCommand
        {
            WebServiceToken = token
        });
    }
}
using AdLerBackend.API.Properties;
using AdLerBackend.Application.Common.Responses.Player;
using AdLerBackend.Application.Player.DeletePlayerData;
using AdLerBackend.Application.Player.GetPlayerData;
using AdLerBackend.Application.Player.UpdatePlayerData;
using AdLerBackend.Domain.Entities.PlayerData;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdLerBackend.API.Controllers.Player;

/// <summary>
///     Player controller
/// </summary>
[Route("api/PlayerData")]
public class PlayerController : BaseApiController
{
    private readonly BackendConfig _config;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="mediator"></param>
    public PlayerController(IMediator mediator, IOptions<BackendConfig> config) : base(mediator)
    {
        _config = config.Value;
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

    [HttpGet("test")]
    public async Task<ActionResult<string>> GetDbPassword()
    {
        return _config.ASPNETCORE_ADLER_TESTVARIABLE;
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
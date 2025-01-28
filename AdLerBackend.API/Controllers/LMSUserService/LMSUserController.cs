using System.ComponentModel.DataAnnotations;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LMS.GetLMSToken;
using AdLerBackend.Application.LMS.GetUserData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

namespace AdLerBackend.API.Controllers.LMSUserService;

[Route("api/Users")]
[ApiController]
public class LmsLoginController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("UserData")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<LMSUserDataResponse>> GetLmsUserData(
        [FromQuery] GetLMSUserDataCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("Login")]
    [Obsolete("This method is deprecated and will be removed in future versions. Use the POST method instead.")]
    public async Task<ActionResult<LMSUserTokenResponse>> GetLmsUserTokenWithGet(
        [FromQuery] GetLMSTokenCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<LMSUserTokenResponse>> GetLmsUserToken(
        [FromBody] [Required] GetLMSTokenCommand command)
    {
        return await Mediator.Send(command);
    }
}
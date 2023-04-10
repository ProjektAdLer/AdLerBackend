using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LMS.GetLMSToken;
using AdLerBackend.Application.LMS.GetUserData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

namespace AdLerBackend.API.Controllers.LMSUserService;

[Route("api/Users")]
public class LmsLoginController : BaseApiController
{
    public LmsLoginController(IMediator mediator) : base(mediator)
    {
    }

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
    public async Task<ActionResult<LMSUserTokenResponse>> GetLmsUserToken(
        [FromQuery] GetLMSTokenCommand command)
    {
        return await Mediator.Send(command);
    }
}
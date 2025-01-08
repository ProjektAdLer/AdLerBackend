using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LMS.GetLMSToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

namespace AdLerBackend.API.Controllers.LMSUserService;

[Route("api/Users")]
public class LmsLoginController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("Login")]
    public async Task<ActionResult<LMSUserTokenResponse>> GetLmsUserToken(
        [FromQuery] GetLMSTokenCommand command)
    {
        return await Mediator.Send(command);
    }
}
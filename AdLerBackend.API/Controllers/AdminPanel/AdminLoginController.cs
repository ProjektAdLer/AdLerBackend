using System.ComponentModel.DataAnnotations;
using AdLerBackend.Application.AdminPanel.GetAdminToken;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.API.Controllers.AdminPanel;

[Route("api/admin")]
[ApiController]
public class AdminLoginController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpPost("Login")]
    /// <summary>
    ///     This endpoint is used to login to the LMS as an admin user.
    /// </summary>
    public async Task<LMSUserTokenResponse> Login([FromBody] [Required] GetAdminTokenCommand command)
    {
        return await mediator.Send(command);
    }
}
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
    public async Task<LMSUserTokenResponse> Login([FromBody] [Required] GetAdminTokenCommand command)
    {
        return await mediator.Send(command);
    }
}
using AdLerBackend.Application.AdminPanel.GetAdminToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.API.Controllers.AdminPanel;

[Microsoft.AspNetCore.Components.Route("api/Login")]
[ApiController]
public class AdminUserController(IMediator mediator) : BaseApiController(mediator)
{
    /// <summary>
    ///     Create a List of Users by Csv File
    /// </summary>
    [HttpPost("Users")]
    public async Task<IActionResult> Login([FromBody] GetAdminTokenCommand command)
    {
        throw new NotImplementedException();
    }
}
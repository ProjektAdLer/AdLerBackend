using AdLerBackend.Application.AdminPanel.CreateUsersByCsv;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.API.Controllers.AdminPanel;

[Route("api/admin")]
[ApiController]
public class AdminUserController(IMediator mediator) : BaseApiController(mediator)
{
    /// <summary>
    ///     Create a List of Users by Csv File
    /// </summary>
    [HttpPost("Users")]
    public async Task<bool> Login(IFormFile userFile, [FromHeader] string token)
    {
        return await Mediator.Send(
            new CreateUsersByCsvCommand
            {
                UserFileStream = userFile.OpenReadStream(),
                WebServiceToken = token
            });
    }
}
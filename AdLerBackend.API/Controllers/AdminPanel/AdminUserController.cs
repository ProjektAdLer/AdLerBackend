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
    public async Task<bool> CreateUsers(IFormFile userFile, [FromHeader] string token)
    {
        await using var stream = userFile.OpenReadStream();
        return await Mediator.Send(new CreateUsersByCsvCommand
        {
            UserFileStream = stream,
            WebServiceToken = token
        });
    }
}
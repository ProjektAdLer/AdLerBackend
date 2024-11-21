using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.World.GetWorldDetail;
using AdLerBackend.Application.World.GetWorldsForAuthor;
using AdLerBackend.Application.World.GetWorldsForUser;
using AdLerBackend.Application.World.GetWorldStatus;
using AdLerBackend.Application.World.WorldManagement.DeleteWorld;
using AdLerBackend.Application.World.WorldManagement.UploadWorld;
using MediatR;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1591

#pragma warning disable CS1573

namespace AdLerBackend.API.Controllers.Worlds;

/// <summary>
///     Manages all the Worlds
/// </summary>
[Route("api/Worlds")]
public class WorldsController : BaseApiController
{
    public WorldsController(IMediator mediator) : base(mediator)
    {
    }


    /// <summary>
    ///     Gets all Worlds that a Author has created
    /// </summary>
    /// <param name="token"></param>
    /// <param name="authorId"></param>
    /// <returns></returns>
    [HttpGet("author/{authorId}")]
    public async Task<ActionResult<GetWorldOverviewResponse>> GetWorldsForAuthor([FromHeader] string token,
        int authorId)
    {
        return Ok(await Mediator.Send(new GetWorldsForAuthorCommand
        {
            AuthorId = authorId,
            WebServiceToken = token
        }));
    }

    /// <summary>
    ///     Gets the ATF File of a World
    /// </summary>
    /// <returns></returns>
    [HttpGet("{worldId}")]
    public async Task<ActionResult<WorldAtfResponse>> GetWorldAtf([FromHeader] string token,
        [FromRoute] int worldId)

    {
        return await Mediator.Send(new GetWorldDetailCommand
        {
            WorldId = worldId,
            WebServiceToken = token
        });
    }

    /// <summary>
    ///     Gets All Worlds a User is enrolled in
    /// </summary>
    /// <param name="token">The Users WebService Token</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<GetWorldOverviewResponse>> GetWorldsForUser([FromHeader] string token)
    {
        return await Mediator.Send(new GetWorldsForUserCommand
        {
            WebServiceToken = token
        });
    }

    /// <summary>
    ///     Uploads a World to the Backend
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<CreateWorldResponse> CreateWorld(IFormFile backupFile, IFormFile atfFile,
        [FromHeader] string token)
    {
        var ret = await Mediator.Send(new UploadWorldCommand
        {
            BackupFileStream = backupFile.OpenReadStream(),
            ATFFileStream = atfFile.OpenReadStream(),
            WebServiceToken = token
        });

        return ret;
    }

    /// <summary>
    ///     Deletes a World by its Id
    /// </summary>
    /// <param name="token"></param>
    /// <param name="worldId"></param>
    /// <returns></returns>
    [HttpDelete("{worldId}")]
    public async Task<bool> DeleteWorld([FromHeader] string token, [FromRoute] int worldId)
    {
        return await Mediator.Send(new DeleteWorldCommand
        {
            WorldId = worldId,
            WebServiceToken = token
        });
    }

    /// <summary>
    ///     Gets the Status of all Elements in a World
    /// </summary>
    /// <param name="token"></param>
    /// <param name="worldId"></param>
    /// <returns></returns>
    [HttpGet("{worldId}/Status")]
    public async Task<ActionResult<WorldStatusResponse>> GetElementStatus([FromHeader] string token,
        [FromRoute] int worldId)
    {
        return await Mediator.Send(new GetWorldStatusCommand
        {
            WorldId = worldId,
            WebServiceToken = token
        });
    }
}
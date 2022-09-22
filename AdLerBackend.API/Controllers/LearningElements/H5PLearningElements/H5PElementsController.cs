using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.LearningElement.H5P.ScoreH5PElement;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.API.Controllers.LearningElements.H5PLearningElements;

[Route("api/LearningElements/H5P")]
public class H5PElementsController : BaseApiController
{
    public H5PElementsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPatch("{h5pId}")]
    public async Task<ActionResult<ScoreLearningElementResponse>> ScoreH5PElement([FromHeader] string token,
        [FromRoute] int h5pId, [FromBody] RawH5PEvent h5pEvent, string userEmail, string userName, string h5pName)
    {
        return await Mediator.Send(new ScoreH5PElementCommand
        {
            WebServiceToken = token,
            H5PId = h5pId,
            H5PEvent = h5pEvent,
            UserEmail = userEmail,
            UserName = userName,
            H5PName = h5pName
        });
    }
}
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
        [FromRoute] int h5pId, [FromBody] ScoreH5PElementParams scoreH5PElementParams)
    {
        return await Mediator.Send(new ScoreH5PElementCommand
        {
            WebServiceToken = token,
            H5PId = h5pId,
            serializedXAPIEvent = scoreH5PElementParams.serializedXAPIEvent,
            UserEmail = scoreH5PElementParams.userEmail,
            UserName = scoreH5PElementParams.userName,
            H5PName = scoreH5PElementParams.h5pName
        });
    }
}
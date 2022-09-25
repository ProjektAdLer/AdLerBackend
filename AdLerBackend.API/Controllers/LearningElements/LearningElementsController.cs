using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.LearningElement.GetLearningElementScore;
using AdLerBackend.Application.LearningElement.H5P.ScoreH5PElement;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.API.Controllers.LearningElements;

[Route("api/LearningElements/")]
public class LearningElementsController : BaseApiController
{
    public LearningElementsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPatch("Course/{courseId}/Element/{elementId}")]
    public async Task<ActionResult<ScoreLearningElementResponse>> ScoreElement([FromHeader] string token,
        [FromRoute] int elementId, [FromRoute] int courseId, [FromBody] ScoreElementParams scoreElementParams)
    {
        return await Mediator.Send(new ScoreElementCommand
        {
            WebServiceToken = token,
            ElementId = elementId,
            CourseId = courseId,
            ScoreElementParams = scoreElementParams
        });
    }

    [HttpGet("Course/{courseId}/Element/{elementId}/Score")]
    public async Task<LearningElementScoreResponse> GetElementScore([FromHeader] string token,
        [FromRoute] int elementId, [FromRoute] int courseId)
    {
        return await Mediator.Send(new GetLearningElementScoreCommand
        {
            WebServiceToken = token,
            learningElementId = elementId,
            lerningWorldId = courseId
        });
    }
}
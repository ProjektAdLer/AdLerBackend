using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.LearningElement.GetLearningElementScore;
using AdLerBackend.Application.LearningElement.GetLearningElementSource;
using AdLerBackend.Application.LearningElement.ScoreLearningElement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdLerBackend.API.Controllers.LearningElements;

[Route("api/LearningElements/")]
public class LearningElementsController : BaseApiController
{
    public LearningElementsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPatch("Course/{courseId}/Element/{elementId}")]
    public async Task<ActionResult<ScoreLearningElementResponse>> ScoreElement([FromHeader] string token,
        [FromRoute] int elementId, [FromRoute] int courseId,
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        ScoreElementParams? scoreElementParams)
    {
        return await Mediator.Send(new ScoreLearningElementCommand
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

    [HttpGet("FilePath/Course/{courseId}/Element/{id}")]
    public async Task<ActionResult<GetLearningElementSourceResponse>> GetLearningElementSource(
        [FromHeader] string token,
        [FromRoute] int id, [FromRoute] int courseId)
    {
        return await Mediator.Send(new GetLearningElementSourceCommand
        {
            ElementId = id,
            WebServiceToken = token,
            CourseId = courseId
        });
    }
}
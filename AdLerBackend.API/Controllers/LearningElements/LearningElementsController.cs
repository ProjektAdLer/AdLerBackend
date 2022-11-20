using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.LearningElement.GetLearningElementScore;
using AdLerBackend.Application.LearningElement.GetLearningElementSource;
using AdLerBackend.Application.LearningElement.ScoreLearningElement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdLerBackend.API.Controllers.LearningElements;

/// <summary>
///     Controller for learning elements
/// </summary>
[Route("v1")]
public class LearningElementsController : BaseApiController
{
    public LearningElementsController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    ///     Try to Score a Learning Element
    /// </summary>
    /// <param name="token">LMS User Token</param>
    /// <param name="elementId">Id of the Element in the Course</param>
    /// <param name="courseId">Id of the Course</param>
    /// <param name="scoreElementParams">Params needed for scoring uf h5p Elements</param>
    /// <returns></returns>
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

    /// <summary>
    ///     Gets a Score for the Learning Element
    /// </summary>
    /// <param name="token">Lms Webservice Token</param>
    /// <param name="elementId">Id of a Element</param>
    /// <param name="courseId">Id of the Course the Element is located in</param>
    /// <returns></returns>
    [HttpGet("Course/{courseId}/LearningElement/{elementId}/Score")]
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

    /// <summary>
    ///     Gets the Source of a Learning Element, which is needed to display it
    /// </summary>
    /// <param name="token">Lms Webservice Token</param>
    /// <param name="elementId">Id of a Element</param>
    /// <param name="courseId">Id of the Course the Element is located in</param>
    /// <returns></returns>
    [HttpGet("FilePath/Course/{courseId}/LearningElement/{elementId}/Source")]
    public async Task<ActionResult<GetLearningElementSourceResponse>> GetLearningElementSource(
        [FromHeader] string token,
        [FromRoute] int elementId, [FromRoute] int courseId)
    {
        return await Mediator.Send(new GetLearningElementSourceCommand
        {
            ElementId = elementId,
            WebServiceToken = token,
            CourseId = courseId
        });
    }
}
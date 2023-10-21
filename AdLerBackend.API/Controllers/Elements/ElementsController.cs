using AdLerBackend.Application.Adaptivity.AnswerAdaptivityQuestion;
using AdLerBackend.Application.Adaptivity.GetAdaptivityModuleQuestionDetails;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.Adaptivity;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Element.GetElementScore;
using AdLerBackend.Application.Element.GetElementSource;
using AdLerBackend.Application.Element.ScoreElement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdLerBackend.API.Controllers.Elements;

/// <summary>
///     Controller for learning elements
/// </summary>
[Route("api/Elements")]
public class ElementsController : BaseApiController
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="mediator"></param>
    public ElementsController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    ///     Try to Score a Learning Element
    /// </summary>
    /// <param name="token">LMS User Token</param>
    /// <param name="elementId">Id of the Element in the World</param>
    /// <param name="worldId">Id of the World</param>
    /// <param name="scoreElementParams">Params needed for scoring uf h5p Elements</param>
    /// <returns></returns>
    [HttpPatch("World/{worldId}/Element/{elementId}")]
    public async Task<ActionResult<ScoreElementResponse>> ScoreElement([FromHeader] string token,
        [FromRoute] int elementId, [FromRoute] int worldId,
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        ScoreElementParams? scoreElementParams)
    {
        return await Mediator.Send(new ScoreElementCommand
        {
            WebServiceToken = token,
            ElementId = elementId,
            WorldId = worldId,
            ScoreElementParams = scoreElementParams
        });
    }

    /// <summary>
    ///     Answer a Question in a an Adaptivity Learning Element
    /// </summary>
    /// <returns></returns>
    [HttpPatch("World/{worldId}/Element/{elementId}/Question/{questionId}")]
    public async Task<ActionResult<AnswerAdaptivityQuestionResponse>> AnswerAdaptivityQuestion(
        [FromHeader] string token, int worldId,
        int elementId, int questionId,
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        bool[] answers)
    {
        return await Mediator.Send(new AnswerAdaptivityQuestionCommand
        {
            ElementId = elementId,
            WebServiceToken = token,
            WorldId = worldId,
            Answers = answers,
            QuestionId = questionId
        });
    }

    /// <summary>
    ///     Get the Details of an Adaptivity Element
    /// </summary>
    /// <returns></returns>
    [HttpGet("World/{worldId}/Element/{elementId}/Adaptivity")]
    public async Task<GetAdaptivityElementDetailsResponse> GetAdaptivityQuestions(
        [FromHeader] string token, int worldId,
        int elementId)
    {
        return await Mediator.Send(new GetAdaptivityElementDetailsCommand
        {
            ElementId = elementId,
            WebServiceToken = token,
            LearningWorldId = worldId
        });
    }

    /// <summary>
    ///     Gets a Score for the Learning Element
    /// </summary>
    /// <param name="token">Lms Webservice Token</param>
    /// <param name="elementId">Id of a Element</param>
    /// <param name="worldId">Id of the World the Element is located in</param>
    /// <returns></returns>
    [HttpGet("World/{worldId}/Element/{elementId}/Score")]
    public async Task<ElementScoreResponse> GetElementScore([FromHeader] string token,
        [FromRoute] int elementId, [FromRoute] int worldId)
    {
        return await Mediator.Send(new GetElementScoreCommand
        {
            WebServiceToken = token,
            ElementId = elementId,
            LearningWorldId = worldId
        });
    }

    /// <summary>
    ///     Gets the Source of a Learning Element, which is needed to display it
    /// </summary>
    /// <param name="token">Lms Webservice Token</param>
    /// <param name="elementId">Id of a Element</param>
    /// <param name="worldId">Id of the World the Element is located in</param>
    /// <returns></returns>
    [HttpGet("FilePath/World/{worldId}/Element/{elementId}")]
    public async Task<ActionResult<GetElementSourceResponse>> GetElementSource(
        [FromHeader] string token,
        [FromRoute] int elementId, [FromRoute] int worldId)
    {
        return await Mediator.Send(new GetElementSourceCommand
        {
            ElementId = elementId,
            WebServiceToken = token,
            WorldId = worldId
        });
    }
}
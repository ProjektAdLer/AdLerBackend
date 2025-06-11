using AdLerBackend.Application.Adaptivity.AnswerAdaptivityQuestion;
using AdLerBackend.Application.Adaptivity.GetAdaptivityModuleQuestionDetails;
using AdLerBackend.Application.Common.Responses.Adaptivity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdLerBackend.API.Controllers.Elements;

/// <summary>
///     Controller for adaptivity elements
/// </summary>
[Route("api/Elements")]
public class AdaptivityElementController : BaseApiController
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="mediator"></param>
    public AdaptivityElementController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    ///     Answer a Question in an Adaptivity Learning Element main test
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
}
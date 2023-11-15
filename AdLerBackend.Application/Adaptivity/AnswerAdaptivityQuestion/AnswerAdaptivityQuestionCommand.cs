using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Adaptivity;

namespace AdLerBackend.Application.Adaptivity.AnswerAdaptivityQuestion;

public record AnswerAdaptivityQuestionCommand : CommandWithToken<AnswerAdaptivityQuestionResponse>
{
    public int ElementId { get; set; }
    public int WorldId { get; set; }
    public int QuestionId { get; set; }

    /// <summary>
    ///     IMPORTANT: Index matters!
    /// </summary>
    public IList<bool> Answers { get; set; } = new List<bool>();
}
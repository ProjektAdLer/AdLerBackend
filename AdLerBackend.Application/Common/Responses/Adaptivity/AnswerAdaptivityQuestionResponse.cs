using AdLerBackend.Application.Common.Responses.Adaptivity.Common;
using AdLerBackend.Application.Common.Responses.Elements;
using JetBrains.Annotations;

namespace AdLerBackend.Application.Common.Responses.Adaptivity;

[UsedImplicitly]
public record AnswerAdaptivityQuestionResponse
{
    public required ElementScoreResponse ElementScore { get; set; }
    public required IList<GradedTask> GradedTasks { get; set; } = new List<GradedTask>();
    public required IList<GradedQuestion> GradedQuestions { get; set; } = new List<GradedQuestion>();
}
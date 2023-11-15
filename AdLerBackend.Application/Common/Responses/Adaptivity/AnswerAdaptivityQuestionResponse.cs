using AdLerBackend.Application.Common.Responses.Adaptivity.Common;
using AdLerBackend.Application.Common.Responses.Elements;
using JetBrains.Annotations;

namespace AdLerBackend.Application.Common.Responses.Adaptivity;

[UsedImplicitly]
public record AnswerAdaptivityQuestionResponse
{
    public required ElementScoreResponse ElementScore { get; set; }
    public required GradedTask GradedTask { get; set; }
    public required GradedQuestion GradedQuestion { get; set; }
}
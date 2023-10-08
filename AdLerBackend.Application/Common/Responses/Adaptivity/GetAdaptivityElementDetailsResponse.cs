using AdLerBackend.Application.Common.Responses.Adaptivity.Common;
using AdLerBackend.Application.Common.Responses.Elements;
using JetBrains.Annotations;

namespace AdLerBackend.Application.Common.Responses.Adaptivity;

[UsedImplicitly]
public record GetAdaptivityElementDetailsResponse
{
    public ElementScoreResponse Element { get; set; }
    public IList<GradedQuestion> Questions { get; set; }
    public IList<GradedTask> Tasks { get; set; }
}
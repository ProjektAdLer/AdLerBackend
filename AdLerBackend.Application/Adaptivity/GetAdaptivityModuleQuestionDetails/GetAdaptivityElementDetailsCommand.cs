using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.Adaptivity;

namespace AdLerBackend.Application.Adaptivity.GetAdaptivityModuleQuestionDetails;

public record GetAdaptivityElementDetailsCommand : CommandWithToken<GetAdaptivityElementDetailsResponse>
{
    public int LearningWorldId { get; init; }
    public int ElementId { get; init; }
}
using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.LearningElements;

namespace AdLerBackend.Application.LearningElement.H5P.ScoreH5PElement;

public record ScoreH5PElementCommand : CommandWithToken<ScoreLearningElementResponse>
{
    public int H5PId { get; init; }
    public string serializedXAPIEvent { get; init; }
}
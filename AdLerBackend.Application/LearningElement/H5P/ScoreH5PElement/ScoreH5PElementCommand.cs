using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.LearningElements;

namespace AdLerBackend.Application.LearningElement.H5P.ScoreH5PElement;

public record ScoreH5PElementCommand : CommandWithToken<ScoreLearningElementResponse>
{
    public int H5PId { get; init; }
    public RawH5PEvent H5PEvent { get; init; }
    public string UserEmail { get; init; }
    public string UserName { get; init; }
    public string H5PName { get; init; }
}
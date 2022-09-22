using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.LearningElement.H5P.ScoreH5PElement;

public class ScoreH5PElementHandler : IRequestHandler<ScoreH5PElementCommand, ScoreLearningElementResponse>
{
    public Task<ScoreLearningElementResponse> Handle(ScoreH5PElementCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
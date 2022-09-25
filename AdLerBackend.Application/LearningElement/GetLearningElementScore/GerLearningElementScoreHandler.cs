using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.LearningElement.GetLearningElementScore;

public class
    GerLearningElementScoreHandler : IRequestHandler<GetLearningElementScoreCommand, LearningElementScoreResponse>
{
    public Task<LearningElementScoreResponse> Handle(GetLearningElementScoreCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new LearningElementScoreResponse
        {
            successss = true,
            ElementId = 123
        });
    }
}
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.Common.LearningElementStrategies.GenericLearningElementStrategy;

public class
    GenericLearningElementStrategyHandler : IRequestHandler<GenericLearningElementStrategyCommand,
        LearningElementScoreResponse>
{
    public Task<LearningElementScoreResponse> Handle(GenericLearningElementStrategyCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new LearningElementScoreResponse
        {
            successss = request.LearningElementMoule.CompletionData.State == 1
        });
    }
}
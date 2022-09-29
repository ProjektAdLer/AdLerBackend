using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.Common.LearningElementStrategies.GetLearningElementScoreStrategies.GenericGetLearningElementScoreStrategy;

public class
    GenericGetLearningElementScoreStrategyHandler : IRequestHandler<GenericGetLearningElementScoreScoreStrategyCommand,
        LearningElementScoreResponse>
{
    public Task<LearningElementScoreResponse> Handle(GenericGetLearningElementScoreScoreStrategyCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new LearningElementScoreResponse
        {
            successss = request.LearningElementMoule.CompletionData.State == 1
        });
    }
}
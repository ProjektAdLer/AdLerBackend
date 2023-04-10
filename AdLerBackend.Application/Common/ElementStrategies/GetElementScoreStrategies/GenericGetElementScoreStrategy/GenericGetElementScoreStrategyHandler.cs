using AdLerBackend.Application.Common.Responses.Elements;
using MediatR;

namespace AdLerBackend.Application.Common.ElementStrategies.GetElementScoreStrategies.
    GenericGetElementScoreStrategy;

public class
    GenericGetLearningElementScoreStrategyHandler : IRequestHandler<GenericGetElementScoreScoreStrategyCommand,
        ElementScoreResponse>
{
    public Task<ElementScoreResponse> Handle(GenericGetElementScoreScoreStrategyCommand request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new ElementScoreResponse
        {
            ElementId = request.ElementId,
            Success = request.ElementModule.CompletionData.State == 1
        });
    }
}
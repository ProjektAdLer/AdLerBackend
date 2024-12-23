using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Elements;
using MediatR;

namespace AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.
    ScoreGenericLearningElementStrategy;

public class ScoreGenericElementStrategyHandler(ILMS ilms) : IRequestHandler<ScoreGenericElementStrategyCommand,
    ScoreElementResponse>
{
    public async Task<ScoreElementResponse> Handle(ScoreGenericElementStrategyCommand request,
        CancellationToken cancellationToken)
    {
        return new ScoreElementResponse
        {
            IsSuccess = await ilms.ScoreGenericElementViaPlugin(request.WebServiceToken, request.LmsModule.Id)
        };
    }
}
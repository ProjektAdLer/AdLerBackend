using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Elements;
using MediatR;

namespace AdLerBackend.Application.Common.ElementStrategies.GetElementScoreStrategies.
    GetH5PElementScoreStrategy;

public class
    GetH5PElementScoreStrategyHandler : IRequestHandler<GetH5PElementScoreStrategyCommand,
        ElementScoreResponse>
{
    private readonly ILMS _ilms;

    public GetH5PElementScoreStrategyHandler(ILMS ilms)
    {
        _ilms = ilms;
    }


    public async Task<ElementScoreResponse> Handle(GetH5PElementScoreStrategyCommand request,
        CancellationToken cancellationToken)
    {
        var instanceId = request.ElementMoule.Instance;

        var allUserAttempts = await _ilms.GetH5PAttemptsAsync(request.WebServiceToken, instanceId);

        var success = allUserAttempts?.usersattempts[0]?.scored?.attempts[0]?.success ?? 0;

        return new ElementScoreResponse
        {
            Success = success == 1,
            ElementId = request.ElementId
        };
    }
}
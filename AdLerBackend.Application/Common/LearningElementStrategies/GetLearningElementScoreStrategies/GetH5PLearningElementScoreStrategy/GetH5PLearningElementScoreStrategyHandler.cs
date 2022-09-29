using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.Common.LearningElementStrategies.GetLearningElementScoreStrategies.
    GetH5PLearningElementScoreStrategy;

public class
    GetH5PLearningElementScoreStrategyHandler : IRequestHandler<GetH5PLearningElementScoreStrategyCommand,
        LearningElementScoreResponse>
{
    private readonly IMoodle _moodle;

    public GetH5PLearningElementScoreStrategyHandler(IMoodle moodle)
    {
        _moodle = moodle;
    }


    public async Task<LearningElementScoreResponse> Handle(GetH5PLearningElementScoreStrategyCommand request,
        CancellationToken cancellationToken)
    {
        var instanceId = request.LearningElementMoule.Instance;

        var allUserAttempts = await _moodle.GetH5PAttemptsAsync(request.WebServiceToken, instanceId);

        var success = allUserAttempts?.usersattempts[0]?.scored?.attempts[0]?.success ?? 0;

        return new LearningElementScoreResponse
        {
            successss = success == 1,
            ElementId = request.ElementId
        };
    }
}
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.Common.LearningElementStrategies.H5PLearningElementStrategy;

public class
    H5PLearningElementStrategieHandler : IRequestHandler<H5PLearningElementStrategyCommand,
        LearningElementScoreResponse>
{
    private readonly IMoodle _moodle;

    public H5PLearningElementStrategieHandler(IMoodle moodle)
    {
        _moodle = moodle;
    }


    public async Task<LearningElementScoreResponse> Handle(H5PLearningElementStrategyCommand request,
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
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.
    ScoreGenericLearningElementStrategy;

public class ScoreGenericLearningElementStrategyHandler : IRequestHandler<ScoreGenericLearningElementStrategyCommand,
    ScoreLearningElementResponse>
{
    private readonly IMoodle _moodle;

    public ScoreGenericLearningElementStrategyHandler(IMoodle moodle)
    {
        _moodle = moodle;
    }

    public async Task<ScoreLearningElementResponse> Handle(ScoreGenericLearningElementStrategyCommand request,
        CancellationToken cancellationToken)
    {
        return new ScoreLearningElementResponse
        {
            isSuceess = await _moodle.ScoreGenericLearningElement(request.WebServiceToken, request.Module.Id)
        };
    }
}
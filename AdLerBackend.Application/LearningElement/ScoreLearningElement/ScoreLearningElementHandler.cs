using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElementLmsInformation;
using AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.
    ScoreGenericLearningElementStrategy;
using AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.ScoreH5PStrategy;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.LearningElement.ScoreLearningElement;

public class ScoreLearningElementHandler : IRequestHandler<ScoreLearningElementCommand, ScoreLearningElementResponse>
{
    private readonly IMediator _mediator;

    public ScoreLearningElementHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ScoreLearningElementResponse> Handle(ScoreLearningElementCommand request,
        CancellationToken cancellationToken)
    {
        // Get LearningElement Activity Id
        var learningElementModule = await _mediator.Send(new GetLearningElementLmsInformationCommand
        {
            CourseId = request.CourseId,
            ElementId = request.ElementId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        var elementScoreResponse = await _mediator.Send(GetStrategy(learningElementModule.LearningElementData.ModName,
            new GetStrategyParams
            {
                LearningElementMoule = learningElementModule.LearningElementData,
                WebServiceToken = request.WebServiceToken,
                ScoreElementParams = request.ScoreElementParams ?? new ScoreElementParams()
            }), cancellationToken);

        return new ScoreLearningElementResponse
        {
            isSuceess = elementScoreResponse.isSuceess
        };
    }

    private static CommandWithToken<ScoreLearningElementResponse> GetStrategy(string elementType,
        GetStrategyParams commandWithParams)
    {
        switch (elementType)
        {
            case "h5pactivity":
                return new ScoreH5PElementStrategyCommand
                {
                    Module = commandWithParams.LearningElementMoule,
                    ScoreElementParams = commandWithParams.ScoreElementParams,
                    WebServiceToken = commandWithParams.WebServiceToken
                };
            case "url":
            case "resource":
                return new ScoreGenericLearningElementStrategyCommand
                {
                    Module = commandWithParams.LearningElementMoule,
                    WebServiceToken = commandWithParams.WebServiceToken
                };
            default:
                throw new NotImplementedException("Strategy for this element type is not implemented " + elementType);
        }
    }

#pragma warning disable CS8618
    private class GetStrategyParams
    {
        public string WebServiceToken { get; set; }
        public ScoreElementParams ScoreElementParams { get; init; }
        public Modules LearningElementMoule { get; init; }
    }
#pragma warning restore CS8618
}
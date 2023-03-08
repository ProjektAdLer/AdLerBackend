using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreGenericLearningElementStrategy;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreH5PStrategy;
using AdLerBackend.Application.Common.InternalUseCases.GetElementLmsInformation;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.Element.ScoreElement;

public class ScoreElementHandler : IRequestHandler<ScoreElementCommand, ScoreElementResponse>
{
    private readonly IMediator _mediator;

    public ScoreElementHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ScoreElementResponse> Handle(ScoreElementCommand request,
        CancellationToken cancellationToken)
    {
        // Get LearningElement Activity Id
        var learningElementModule = await _mediator.Send(new GetElementLmsInformationCommand
        {
            WorldId = request.WorldId,
            ElementId = request.ElementId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        var elementScoreResponse = await _mediator.Send(GetStrategy(learningElementModule.ElementData.ModName,
            new GetStrategyParams
            {
                LearningElementMoule = learningElementModule.ElementData,
                WebServiceToken = request.WebServiceToken,
                ScoreElementParams = request.ScoreElementParams ?? new ScoreElementParams()
            }), cancellationToken);

        return new ScoreElementResponse
        {
            IsSuccess = elementScoreResponse.IsSuccess
        };
    }

    private static CommandWithToken<ScoreElementResponse> GetStrategy(string elementType,
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
                return new ScoreGenericElementStrategyCommand
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
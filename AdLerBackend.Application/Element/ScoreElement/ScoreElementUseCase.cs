using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.MockPrimitiveH5PStrategy;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreGenericLearningElementStrategy;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreH5PStrategy;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using JetBrains.Annotations;
using MediatR;

namespace AdLerBackend.Application.Element.ScoreElement;

/// <summary>
///     Use case for scoring an element
///     It Gets the learning element from the LMS Corelates the element with the ATF and scores the element in the LMS via
///     the Plugin
/// </summary>
[UsedImplicitly]
public class ScoreElementUseCase : IRequestHandler<ScoreElementCommand, ScoreElementResponse>
{
    private readonly IMediator _mediator;

    public ScoreElementUseCase(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ScoreElementResponse> Handle(ScoreElementCommand request,
        CancellationToken cancellationToken)
    {
        var learningElementModule = await _mediator.Send(new GetLearningElementCommand
        {
            ElementId = request.ElementId,
            WorldId = request.WorldId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        var elementScoreResponse = await _mediator.Send(GetStrategy(learningElementModule.AdLerElement.ElementCategory,
            new GetStrategyParams
            {
                LearningElementMoule = learningElementModule.LmsModule,
                WebServiceToken = request.WebServiceToken,
                ScoreElementParams = request.ScoreElementParams ?? new ScoreElementParams()
            }, request.ElementId), cancellationToken);

        return new ScoreElementResponse
        {
            IsSuccess = elementScoreResponse.IsSuccess
        };
    }

    private static CommandWithToken<ScoreElementResponse> GetStrategy(string elementType,
        GetStrategyParams commandWithParams, int mockId)
    {
        switch (elementType)
        {
            case "h5p":
                return new ScoreH5PElementStrategyCommand
                {
                    LmsModule = commandWithParams.LearningElementMoule,
                    ScoreElementParams = commandWithParams.ScoreElementParams,
                    WebServiceToken = commandWithParams.WebServiceToken
                };
            case "image":
            case "text":
            case "pdf":
            case "video":
            case "primitiveH5P":
                return new ScoreGenericElementStrategyCommand
                {
                    LmsModule = commandWithParams.LearningElementMoule,
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
        public LmsModule LearningElementMoule { get; init; }
    }
#pragma warning restore CS8618
}
using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreGenericLearningElementStrategy;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreH5PStrategy;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.Element.ScoreElement;

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
        var learningElementModules = await _mediator.Send(new GetAllElementsFromLmsCommand
        {
            WorldId = request.WorldId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        // Get LearningElement Activity Id
        var learningElementModule = learningElementModules.ModulesWithAdLerId
            .FirstOrDefault(x => x.AdLerId == request.ElementId);

        if (learningElementModule == null || learningElementModule!.IsLocked)
            throw new NotFoundException("Element not found or locked");

        var elementScoreResponse = await _mediator.Send(GetStrategy(learningElementModule.LmsModule.ModName,
            new GetStrategyParams
            {
                LearningElementMoule = learningElementModule.LmsModule,
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
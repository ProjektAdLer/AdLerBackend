using AdLerBackend.Application.Common.ElementStrategies.GetElementScoreStrategies.GenericGetElementScoreStrategy;
using AdLerBackend.Application.Common.InternalUseCases.GetElementLmsInformation;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.World.GetElementStatus;
using MediatR;

namespace AdLerBackend.Application.Element.GetElementScore;

/// <summary>
///     Gets the highes Scoring Attempt for a given Learning Element
/// </summary>
public class
    GetElementScoreHandler : IRequestHandler<GetElementScoreCommand, ElementScoreResponse>
{
    private readonly IMediator _mediator;

    public GetElementScoreHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ElementScoreResponse> Handle(GetElementScoreCommand request,
        CancellationToken cancellationToken)
    {
        // Get LearningElement Activity Id
        var learningElementModule = await _mediator.Send(new GetElementLmsInformationCommand
        {
            WorldId = request.lerningWorldId,
            ElementId = request.ElementId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);


        return await _mediator.Send(
            GetLearningElementStatusHandler.GetStrategy(learningElementModule.ElementData.ModName,
                new GenericGetElementScoreScoreStrategyCommand
                {
                    ElementId = request.ElementId,
                    ElementModule = learningElementModule.ElementData,
                    WebServiceToken = request.WebServiceToken
                })
            , cancellationToken);
    }
}
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElementLmsInformation;
using AdLerBackend.Application.Common.LearningElementStrategies.GetLearningElementScoreStrategies.
    GenericGetLearningElementScoreStrategy;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.Course.GetLearningElementStatus;
using MediatR;

namespace AdLerBackend.Application.LearningElement.GetLearningElementScore;

/// <summary>
///     Gets the highes Scoring Attempt for a given Learning Element
/// </summary>
public class
    GetLearningElementScoreHandler : IRequestHandler<GetLearningElementScoreCommand, LearningElementScoreResponse>
{
    private readonly IMediator _mediator;

    public GetLearningElementScoreHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<LearningElementScoreResponse> Handle(GetLearningElementScoreCommand request,
        CancellationToken cancellationToken)
    {
        // Get LearningElement Activity Id
        var learningElementModule = await _mediator.Send(new GetLearningElementLmsInformationCommand
        {
            CourseId = request.lerningWorldId,
            ElementId = request.learningElementId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);


        return await _mediator.Send(
            GetLearningElementStatusHandler.GetStrategy(learningElementModule.LearningElementData.ModName,
                new GenericGetLearningElementScoreScoreStrategyCommand
                {
                    ElementId = request.learningElementId,
                    LearningElementMoule = learningElementModule.LearningElementData,
                    WebServiceToken = request.WebServiceToken
                })
            , cancellationToken);
    }
}
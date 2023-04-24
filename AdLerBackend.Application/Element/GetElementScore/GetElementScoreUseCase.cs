using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetElementLmsInformation;
using AdLerBackend.Application.Common.Responses.Elements;
using MediatR;

namespace AdLerBackend.Application.Element.GetElementScore;

/// <summary>
///     Gets the highes Scoring Attempt for a given Learning Element
/// </summary>
public class
    GetElementScoreUseCase : IRequestHandler<GetElementScoreCommand, ElementScoreResponse>
{
    private readonly ILMS _lms;
    private readonly IMediator _mediator;

    public GetElementScoreUseCase(IMediator mediator, ILMS lms)
    {
        _mediator = mediator;
        _lms = lms;
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

        var result =
            await _lms.GetElementScoreFromPlugin(request.WebServiceToken, learningElementModule.ElementData.Id);

        return new ElementScoreResponse
        {
            ElementId = request.ElementId,
            Success = result
        };
    }
}
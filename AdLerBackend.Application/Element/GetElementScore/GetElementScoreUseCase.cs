using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
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
        var learningElementModules = await _mediator.Send(new GetAllElementsFromLmsCommand
        {
            WorldId = request.LearningWorldId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        // Get LearningElement Activity Id
        var learningElementModule = learningElementModules.ModulesWithAdLerId
            .FirstOrDefault(x => x.AdLerId == request.ElementId);

        if (learningElementModule == null || learningElementModule!.IsLocked)
            return new ElementScoreResponse
            {
                ElementId = request.ElementId,
                Success = false
            };

        var result =
            await _lms.GetElementScoreFromPlugin(request.WebServiceToken, learningElementModule.LmsModule.Id);

        return new ElementScoreResponse
        {
            ElementId = request.ElementId,
            Success = result
        };
    }
}
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.Elements;
using MediatR;

namespace AdLerBackend.Application.Element.GetElementScore;

/// <summary>
///     Gets the Score of a Learning Element using the LMS Plugin
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
        var learningElementModule = await _mediator.Send(new GetLearningElementCommand
        {
            WebServiceToken = request.WebServiceToken,
            WorldId = request.LearningWorldId,
            ElementId = request.ElementId,
            CanBeLocked = true
        }, cancellationToken);

        if (learningElementModule!.IsLocked)
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
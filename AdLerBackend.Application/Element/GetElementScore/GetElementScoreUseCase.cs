using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.Elements;
using MediatR;

namespace AdLerBackend.Application.Element.GetElementScore;

/// <summary>
///     Gets the Score of a Learning Element using the LMS Plugin
/// </summary>
public class
    GetElementScoreUseCase(IMediator mediator, ILMS lms) : IRequestHandler<GetElementScoreCommand, ElementScoreResponse>
{
    public async Task<ElementScoreResponse> Handle(GetElementScoreCommand request,
        CancellationToken cancellationToken)
    {
        var learningElementModule = await mediator.Send(new GetLearningElementCommand
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
            await lms.GetElementScoreFromPlugin(request.WebServiceToken, learningElementModule.LmsModule.Id);

        return new ElementScoreResponse
        {
            ElementId = request.ElementId,
            Success = result
        };
    }
}
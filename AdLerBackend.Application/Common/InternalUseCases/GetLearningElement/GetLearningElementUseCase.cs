using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;

public class GetLearningElementUseCase : IRequestHandler<GetLearningElementCommand, AdLerLmsElementAggregation>
{
    private readonly IMediator _mediator;

    public GetLearningElementUseCase(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<AdLerLmsElementAggregation> Handle(GetLearningElementCommand request, CancellationToken cancellationToken)
    {
        var learningElementModules = await _mediator.Send(new GetAllElementsFromLmsCommand
        {
            WorldId = request.WorldId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        var learningElementModule = learningElementModules.ElementAggregations
            .FirstOrDefault(x => x.AdLerElement.ElementId == request.ElementId);

        if (learningElementModule == null)
            throw new NotFoundException("Learning Element not found");

        if (!request.CanBeLocked && learningElementModule.IsLocked)
            throw new ForbiddenAccessException("Learning Element is locked and cant be accessed");

        return learningElementModule;
    }
}
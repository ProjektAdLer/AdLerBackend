using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;

public class GetLearningElementUseCase : IRequestHandler<GetLearningElementCommand, ModuleWithId>
{
    private readonly IMediator _mediator;

    public GetLearningElementUseCase(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ModuleWithId> Handle(GetLearningElementCommand request, CancellationToken cancellationToken)
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

        return learningElementModule;
    }
}
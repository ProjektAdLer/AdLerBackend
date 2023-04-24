using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;

namespace AdLerBackend.Application.Common.InternalUseCases.GetElementLmsInformation;

/// <summary>
///     Gets a single Element from Moodle which is specified int the DSL from AMG
/// </summary>
public class GetLearningElementLmsInformationUseCase : IRequestHandler<GetElementLmsInformationCommand,
    GetElementLmsInformationResponse>
{
    private readonly IMediator _mediator;

    public GetLearningElementLmsInformationUseCase(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<GetElementLmsInformationResponse> Handle(GetElementLmsInformationCommand request,
        CancellationToken cancellationToken)
    {
        var module = await _mediator.Send(new GetAllElementsFromLmsCommand
        {
            WebServiceToken = request.WebServiceToken,
            WorldId = request.WorldId
        }, cancellationToken);

        return new GetElementLmsInformationResponse
        {
            ElementData = module.ModulesWithAdLerId
                              .FirstOrDefault(x => x.AdLerId == request.ElementId)?.LmsModule
                          ?? throw new InvalidOperationException()
        };
    }
}
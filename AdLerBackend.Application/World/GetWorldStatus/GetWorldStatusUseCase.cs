using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;

namespace AdLerBackend.Application.World.GetWorldStatus;

public class
    GetWorldStatusUseCase : IRequestHandler<GetWorldStatusCommand, WorldStatusResponse>
{
    private readonly ILMS _ilms;
    private readonly IMediator _mediator;

    public GetWorldStatusUseCase(IMediator mediator, ILMS ilms)
    {
        _mediator = mediator;
        _ilms = ilms;
    }

    public async Task<WorldStatusResponse> Handle(GetWorldStatusCommand request,
        CancellationToken cancellationToken)
    {
        var courseModules = await _mediator.Send(new GetAllElementsFromLmsCommand
        {
            WorldId = request.WorldId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);


        // Get Course Status from LMS 
        var courseStatus =
            await _ilms.GetCourseStatusViaPlugin(request.WebServiceToken, courseModules.LmsCourseId);

        var response = new WorldStatusResponse
        {
            WorldId = request.WorldId,
            Elements = new List<ElementScoreResponse>()
        };

        foreach (var module in courseModules.ElementAggregations)
        {
            // If module is Locked
            if (module.IsLocked)
            {
                response.Elements.Add(new ElementScoreResponse
                {
                    ElementId = module.AdLerElement.ElementId,
                    Success = false
                });
                continue;
            }

            // If LmsModule is not locked
            var elementScore = courseStatus.ElementScores.FirstOrDefault(x => x.ModuleId == module.LmsModule.Id);
            response.Elements.Add(new ElementScoreResponse
            {
                ElementId = module.AdLerElement.ElementId,
                Success = elementScore?.HasScored ?? false
            });
        }

        return response;
    }
}
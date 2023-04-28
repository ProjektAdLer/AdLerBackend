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
        var courseWithAdLerIds = await _mediator.Send(new GetAllElementsFromLmsCommand
        {
            WorldId = request.WorldId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);


        // Get Course Status from LMS 
        var courseStatus =
            await _ilms.GetCourseStatusViaPlugin(request.WebServiceToken, courseWithAdLerIds.LmsCourseId);

        var response = new WorldStatusResponse
        {
            WorldId = request.WorldId,
            Elements = new List<ElementScoreResponse>()
        };

        foreach (var adlerModule in courseWithAdLerIds.ModulesWithAdLerId)
        {
            // If module is Locked
            if (adlerModule.IsLocked)
            {
                response.Elements.Add(new ElementScoreResponse
                {
                    ElementId = adlerModule.AdLerId,
                    Success = false
                });
                continue;
            }

            // If Module is not locked
            var elementScore = courseStatus.ElementScores.FirstOrDefault(x => x.ModuleId == adlerModule.LmsModule.Id);
            response.Elements.Add(new ElementScoreResponse
            {
                ElementId = adlerModule.AdLerId,
                Success = elementScore?.HasScored ?? false
            });
        }

        return response;
    }
}
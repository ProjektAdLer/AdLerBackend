using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.ElementStrategies.GetElementScoreStrategies.GenericGetElementScoreStrategy;
using AdLerBackend.Application.Common.ElementStrategies.GetElementScoreStrategies.GetH5PElementScoreStrategy;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.Elements;
using MediatR;

namespace AdLerBackend.Application.World.GetElementStatus;

public class
    GetLearningElementStatusHandler : IRequestHandler<GetElementStatusCommand, ElementStatusResponse>
{
    private readonly IMediator _mediator;


    public GetLearningElementStatusHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ElementStatusResponse> Handle(GetElementStatusCommand request,
        CancellationToken cancellationToken)
    {
        var resp = new ElementStatusResponse
        {
            WorldId = request.WorldId,
            Elements = new List<ElementScoreResponse>()
        };

        var allModulesInCourseResp = await _mediator.Send(new GetAllElementsFromLmsCommand
        {
            WorldId = request.WorldId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        var allElements = allModulesInCourseResp.ModulesWithID.ToList();

        foreach (var moduleWithId in allElements)
        {
            var response = await _mediator.Send(GetStrategy(moduleWithId.Module.ModName,
                new GenericGetElementScoreScoreStrategyCommand
                {
                    ElementId = moduleWithId.Id,
                    ElementModule = moduleWithId.Module,
                    WebServiceToken = request.WebServiceToken
                }), cancellationToken);

            resp.Elements.Add(response);
        }

        return resp;
    }


    public static CommandWithToken<ElementScoreResponse> GetStrategy(string learningElementType,
        GenericGetElementScoreScoreStrategyCommand commandWithParams)
    {
        switch (learningElementType)
        {
            case "h5pactivity":
                return new GetH5PElementScoreStrategyCommand
                {
                    ElementId = commandWithParams.ElementId,
                    ElementModule = commandWithParams.ElementModule,
                    WebServiceToken = commandWithParams.WebServiceToken
                };
            case "url": return commandWithParams;
            case "resource": return commandWithParams;
            default:
                throw new NotImplementedException(
                    "The learning element type is not implemented: " + learningElementType);
        }
    }
}
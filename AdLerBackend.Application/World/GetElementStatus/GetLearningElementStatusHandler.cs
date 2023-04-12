using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.Elements;
using MediatR;

namespace AdLerBackend.Application.World.GetElementStatus;

public class
    GetLearningElementStatusHandler : IRequestHandler<GetElementStatusCommand, ElementStatusResponse>
{
    private readonly ILMS _ilms;
    private readonly IMediator _mediator;

    public GetLearningElementStatusHandler(IMediator mediator, ILMS ilms)
    {
        _mediator = mediator;
        _ilms = ilms;
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
            var score = await _ilms.GetElementScoreFromPlugin(request.WebServiceToken, moduleWithId.Module.Id);

            resp.Elements.Add(new ElementScoreResponse
            {
                ElementId = moduleWithId.Id,
                Success = score
            });
        }

        return resp;
    }

    //
    // public static CommandWithToken<ElementScoreResponse> GetStrategy(string learningElementType,
    //     GenericGetElementScoreScoreStrategyCommand commandWithParams)
    // {
    //     switch (learningElementType)
    //     {
    //         case "h5pactivity":
    //             return new GetH5PElementScoreStrategyCommand
    //             {
    //                 ElementId = commandWithParams.ElementId,
    //                 ElementMoule = commandWithParams.ElementMoule,
    //                 WebServiceToken = commandWithParams.WebServiceToken
    //             };
    //         case "url": return commandWithParams;
    //         case "resource": return commandWithParams;
    //         default:
    //             throw new NotImplementedException(
    //                 "The learning element type is not implemented: " + learningElementType);
    //     }
    // }
}
using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.InternalUseCases.GetAllLearningElementsFromLms;
using AdLerBackend.Application.Common.LearningElementStrategies.GetLearningElementScoreStrategies.
    GenericGetLearningElementScoreStrategy;
using AdLerBackend.Application.Common.LearningElementStrategies.GetLearningElementScoreStrategies.
    GetH5PLearningElementScoreStrategy;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.Course.GetLearningElementStatus;

public class
    GetLearningElementStatusHandler : IRequestHandler<GetLearningElementStatusCommand, LearningElementStatusResponse>
{
    private readonly IMediator _mediator;


    public GetLearningElementStatusHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<LearningElementStatusResponse> Handle(GetLearningElementStatusCommand request,
        CancellationToken cancellationToken)
    {
        var resp = new LearningElementStatusResponse
        {
            courseId = request.CourseId,
            LearningElements = new List<LearningElementScoreResponse>()
        };

        var allModulesInCourse = await _mediator.Send(new GetAllLearningElementsFromLmsCommand
        {
            CourseId = request.CourseId,
            WebServiceToken = request.WebServiceToken
        });

        var allH5PElements = allModulesInCourse.ModulesWithID.ToList();

        foreach (var moduleWithId in allH5PElements)
        {
            var response = await _mediator.Send(GetStrategy(moduleWithId.Module!.ModName,
                new GenericGetLearningElementScoreScoreStrategyCommand
                {
                    ElementId = moduleWithId.Id,
                    LearningElementMoule = moduleWithId.Module,
                    WebServiceToken = request.WebServiceToken
                }), cancellationToken);

            resp.LearningElements.Add(response);
        }

        return resp;
    }


    public static CommandWithToken<LearningElementScoreResponse> GetStrategy(string learningElementType,
        GenericGetLearningElementScoreScoreStrategyCommand commandWithParams)
    {
        switch (learningElementType)
        {
            case "h5pactivity":
                return new GetH5PLearningElementScoreStrategyCommand
                {
                    ElementId = commandWithParams.ElementId,
                    LearningElementMoule = commandWithParams.LearningElementMoule,
                    WebServiceToken = commandWithParams.WebServiceToken
                };
            default:
                return commandWithParams;
        }
    }
}
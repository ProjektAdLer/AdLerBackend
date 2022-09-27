using AdLerBackend.Application.Common.InternalUseCases.GetAllLearningElementsFromLms;
using AdLerBackend.Application.Common.LearningElementStrategies.H5PLearningElementStrategy;
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

        // Filter all non-H5P elements fron  the list
        var allH5PElements = allModulesInCourse.ModulesWithID.Where(x => x.Module?.ModName == "h5pactivity").ToList();

        foreach (var moduleWithId in allH5PElements)
        {
            var response = await _mediator.Send(new H5PLearningElementStrategyCommand
            {
                ElementId = moduleWithId.Id,
                LearningElementMoule = moduleWithId.Module,
                WebServiceToken = request.WebServiceToken
            }, cancellationToken);

            resp.LearningElements.Add(response);
        }

        return resp;
    }
}
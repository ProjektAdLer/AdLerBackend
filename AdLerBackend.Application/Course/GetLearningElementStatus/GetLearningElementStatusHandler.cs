using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetAllLearningElementsFromLms;
using AdLerBackend.Application.Common.Responses.Course;
using MediatR;

namespace AdLerBackend.Application.Course.GetLearningElementStatus;

public class
    GetLearningElementStatusHandler : IRequestHandler<GetLearningElementStatusCommand, LearningElementStatusResponse>
{
    private readonly IMediator _mediator;
    private readonly IMoodle _moodle;

    public GetLearningElementStatusHandler(IMediator mediator, IMoodle moodle)
    {
        _mediator = mediator;
        _moodle = moodle;
    }

    public async Task<LearningElementStatusResponse> Handle(GetLearningElementStatusCommand request,
        CancellationToken cancellationToken)
    {
        var resp = new LearningElementStatusResponse
        {
            courseId = request.CourseId,
            LearningElements = new List<LearningElementStatus>()
        };

        var allMOdulesInCourse = await _mediator.Send(new GetAllLearningElementsFromLmsCommand
        {
            CourseId = request.CourseId,
            WebServiceToken = request.WebServiceToken
        });

        // Filter all non-H5P elements fron  the list
        var allH5PElements = allMOdulesInCourse.ModulesWithID.Where(x => x.Module?.ModName == "h5pactivity").ToList();

        foreach (var moduleWithId in allH5PElements)
        {
            var test = await _moodle.GetH5PAttemptsAsync(request.WebServiceToken, moduleWithId.Module!.Instance);
            var sucess = test?.usersattempts[0]?.scored?.attempts[0]?.success ?? 0;

            resp.LearningElements.Add(new LearningElementStatus
            {
                ElementId = moduleWithId.Id,
                IsSuccess = sucess == 1
            });
        }

        return resp;
    }
}
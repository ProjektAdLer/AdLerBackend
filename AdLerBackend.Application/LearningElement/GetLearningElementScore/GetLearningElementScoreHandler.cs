using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElementLmsInformation;
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.LearningElement.GetLearningElementScore;

/// <summary>
///     Gets the highes Scoring Attempt for a given Learning Element
/// </summary>
public class
    GetLearningElementScoreHandler : IRequestHandler<GetLearningElementScoreCommand, LearningElementScoreResponse>
{
    private readonly IMediator _mediator;
    private readonly IMoodle _moodle;

    public GetLearningElementScoreHandler(IMediator mediator, IMoodle moodle)
    {
        _mediator = mediator;
        _moodle = moodle;
    }

    public async Task<LearningElementScoreResponse> Handle(GetLearningElementScoreCommand request,
        CancellationToken cancellationToken)
    {
        // Get LearningElement Activity Id
        var learningElementModule = await _mediator.Send(new GetLearningElementLmsInformationCommand
        {
            CourseId = request.lerningWorldId,
            ElementId = request.learningElementId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        if (learningElementModule.LearningElementData.ModName != "h5pactivity")
            throw new Exception(
                "Learning Element is not a H5P Activity, for now (25.09.22) only H5P Activities are supported");

        var instanceId = learningElementModule.LearningElementData.Instance;

        var allUserAttempts = await _moodle.GetH5PAttemptsAsync(request.WebServiceToken, instanceId);


        var success = allUserAttempts?.usersattempts[0]?.scored?.attempts[0]?.success ?? 0;


        return await Task.FromResult(new LearningElementScoreResponse
        {
            successss = success == 1,
            ElementId = request.learningElementId
        });
    }
}
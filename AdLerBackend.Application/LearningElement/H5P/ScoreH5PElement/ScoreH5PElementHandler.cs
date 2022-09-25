using System.Text.Json;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElementLmsInformation;
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.LearningElement.H5P.ScoreH5PElement;

public class ScoreH5PElementHandler : IRequestHandler<ScoreElementCommand, ScoreLearningElementResponse>
{
    private readonly IMediator _mediator;
    private readonly IMoodle _moodle;
    private readonly ISerialization _serialization;

    public ScoreH5PElementHandler(ISerialization serialization, IMoodle moodle,
        IMediator mediator)
    {
        _serialization = serialization;
        _moodle = moodle;
        _mediator = mediator;
    }

    public async Task<ScoreLearningElementResponse> Handle(ScoreElementCommand request,
        CancellationToken cancellationToken)
    {
        // Get User Data
        var userData = await _moodle.GetMoodleUserDataAsync(request.WebServiceToken);

        var mod = await _mediator.Send(new GetLearningElementLmsInformationCommand
        {
            WebServiceToken = request.WebServiceToken,
            CourseId = request.CourseId,
            ElementId = request.ElementId
        }, cancellationToken);

        if (mod.LearningElementData == null)
            throw new NotFoundException("Element with the Id " + request.ElementId + " not found");

        var contextId = mod.LearningElementData.contextid;

        // Deserialize the XAPI Event
        var xapiEvent =
            _serialization.GetObjectFromJsonString<RawH5PEvent>(request.ScoreElementParams.SerializedXapiEvent!);

        xapiEvent.actor.name = userData.MoodleUserName;
        xapiEvent.actor.mbox = userData.UserEmail;

        xapiEvent.@object.id = "https://testmoodle.cluuub.xyz/xapi/activity/" + contextId;

        // serialize the XAPI Event again
        var inText = JsonSerializer.Serialize(xapiEvent);

        // Send the XAPI Event to the LRS
        var isSuccess = await _moodle.ProcessXAPIStatementAsync(request.WebServiceToken, inText);

        return new ScoreLearningElementResponse
        {
            isSuceess = isSuccess
        };
    }
}
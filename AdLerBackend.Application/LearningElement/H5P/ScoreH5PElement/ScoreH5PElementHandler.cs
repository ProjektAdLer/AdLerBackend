using System.Text.Json;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.LearningElement.H5P.ScoreH5PElement;

public class ScoreH5PElementHandler : IRequestHandler<ScoreH5PElementCommand, ScoreLearningElementResponse>
{
    private readonly IMoodle _moodle;
    private readonly ISerialization _serialization;

    public ScoreH5PElementHandler(ISerialization serialization, IMoodle moodle)
    {
        _serialization = serialization;
        _moodle = moodle;
    }

    public async Task<ScoreLearningElementResponse> Handle(ScoreH5PElementCommand request,
        CancellationToken cancellationToken)
    {
        // Get User Data
        var userData = await _moodle.GetMoodleUserDataAsync(request.WebServiceToken);

        // Deserialize the XAPI Event
        var xapiEvent = _serialization.GetObjectFromJsonString<RawH5PEvent>(request.serializedXAPIEvent);

        xapiEvent.actor.name = userData.MoodleUserName;
        xapiEvent.actor.mbox = userData.UserEmail;

        xapiEvent.@object.id = "https://testmoodle.cluuub.xyz/xapi/activity/" + request.H5PId;

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
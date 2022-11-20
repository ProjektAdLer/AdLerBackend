using System.Text.Json;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.ScoreH5PStrategy;

public class
    ScoreH5PElementStrategyHandler : IRequestHandler<ScoreH5PElementStrategyCommand, ScoreLearningElementResponse>
{
    private readonly IConfiguration _config;
    private readonly IMoodle _moodle;
    private readonly ISerialization _serialization;

    public ScoreH5PElementStrategyHandler(ISerialization serialization, IMoodle moodle, IConfiguration config)
    {
        _serialization = serialization;
        _moodle = moodle;
        _config = config;
    }

    public async Task<ScoreLearningElementResponse> Handle(ScoreH5PElementStrategyCommand request,
        CancellationToken cancellationToken)
    {
        // Get User Data
        var userData = await _moodle.GetMoodleUserDataAsync(request.WebServiceToken);

        var contextId = request.Module.contextid;

        // Deserialize the XAPI Event
        var xapiEvent =
            _serialization.GetObjectFromJsonString<RawH5PEvent>(request.ScoreElementParams.SerializedXapiEvent!);


        xapiEvent.actor.name = userData.MoodleUserName;
        xapiEvent.actor.mbox = userData.UserEmail;

        var moodleURL = _config["MoodleURL"];

        // if last character is a slash, remove it
        if (moodleURL[^1] == '/') moodleURL = moodleURL[..^1];

        xapiEvent.@object.id = moodleURL + "/xapi/activity/" + contextId;

        // serialize the XAPI Event again
        var inText = JsonSerializer.Serialize(xapiEvent);

        // Send the XAPI Event to the LRS
        var isSuccess = await _moodle.ProcessXapiStatementAsync(request.WebServiceToken, inText);


        var isAttemptASucess = await _moodle.GetH5PAttemptsAsync(request.WebServiceToken, request.Module.Instance);

        return new ScoreLearningElementResponse
        {
            isSuceess = isAttemptASucess.usersattempts[0].scored.attempts[0].success == 1 && isSuccess
        };
    }
}
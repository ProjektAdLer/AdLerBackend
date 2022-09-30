using System.Text.Json;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LearningElements;
using MediatR;

namespace AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.ScoreH5PStrategy;

public class
    ScoreH5PElementStrategyHandler : IRequestHandler<ScoreH5PElementStrategyCommand, ScoreLearningElementResponse>
{
    private readonly IMoodle _moodle;
    private readonly ISerialization _serialization;

    public ScoreH5PElementStrategyHandler(ISerialization serialization, IMoodle moodle)
    {
        _serialization = serialization;
        _moodle = moodle;
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

        xapiEvent.@object.id = "https://testmoodle.cluuub.xyz/xapi/activity/" + contextId;

        // serialize the XAPI Event again
        var inText = JsonSerializer.Serialize(xapiEvent);

        // Send the XAPI Event to the LRS
        var isSuccess = await _moodle.ProcessXapiStatementAsync(request.WebServiceToken, inText);

        return new ScoreLearningElementResponse
        {
            isSuceess = isSuccess
        };
    }
}
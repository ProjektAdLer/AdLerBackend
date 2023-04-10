using System.Text.Json;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Elements;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreH5PStrategy;

public class
    ScoreH5PElementStrategyHandler : IRequestHandler<ScoreH5PElementStrategyCommand, ScoreElementResponse>
{
    private readonly IConfiguration _config;
    private readonly ILMS _lms;
    private readonly ISerialization _serialization;

    public ScoreH5PElementStrategyHandler(ISerialization serialization, ILMS lms, IConfiguration config)
    {
        _serialization = serialization;
        _lms = lms;
        _config = config;
    }

    public async Task<ScoreElementResponse> Handle(ScoreH5PElementStrategyCommand request,
        CancellationToken cancellationToken)
    {
        // Get User Data
        var userData = await _lms.GetLmsUserDataAsync(request.WebServiceToken);

        var contextId = request.Module.contextid;

        // Deserialize the XApi Event
        var xApiEvent =
            _serialization.GetObjectFromJsonString<RawH5PEvent>(request.ScoreElementParams.SerializedXApiEvent!);


        xApiEvent.Actor.Name = userData.LMSUserName;
        xApiEvent.Actor.Mbox = userData.UserEmail;

        var moodleUrl = _config["MoodleURL"];

        // if moodle url is not set, throw exception
        if (string.IsNullOrEmpty(moodleUrl))
            throw new Exception("Moodle URL is not set in the configuration file");

        // if last character is a slash, remove it
        if (moodleUrl[^1] == '/') moodleUrl = moodleUrl[..^1];

        // ReSharper disable once StringLiteralTypo
        xApiEvent.Object.Id = moodleUrl + "/xapi/activity/" + contextId;

        // serialize the XApi Event again
        var inText = JsonSerializer.Serialize(xApiEvent);

        // Send the XApi Event to the LRS
        var isSuccess = await _lms.ProcessXApiStatementAsync(request.WebServiceToken, inText);


        var isAttemptASuccess = await _lms.GetH5PAttemptsAsync(request.WebServiceToken, request.Module.Instance);

        return new ScoreElementResponse
        {
            IsSuccess = isAttemptASuccess.usersattempts[0].scored.attempts[0].success == 1 && isSuccess
        };
    }
}
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
    private readonly ILMS _ilms;
    private readonly ISerialization _serialization;

    public ScoreH5PElementStrategyHandler(ISerialization serialization, ILMS ilms, IConfiguration config)
    {
        _serialization = serialization;
        _ilms = ilms;
        _config = config;
    }

    public async Task<ScoreElementResponse> Handle(ScoreH5PElementStrategyCommand request,
        CancellationToken cancellationToken)
    {
        var moodleUrl = _config["MoodleURL"];

        // if moodle url is not set, throw exception
        if (string.IsNullOrEmpty(moodleUrl))
            throw new ArgumentException("Moodle URL is not set in the configuration file");

        // if last character is a slash, remove it
        if (moodleUrl[^1] == '/') moodleUrl = moodleUrl[..^1];
        // Get User Data
        var userData = await _ilms.GetLMSUserDataAsync(request.WebServiceToken);

        var contextId = request.Module.contextid;

        // Deserialize the XAPI Event
        var xapiEvent =
            _serialization.GetObjectFromJsonString<RawH5PEvent>(request.ScoreElementParams.SerializedXapiEvent!);


        xapiEvent.actor.name = userData.LMSUserName;
        xapiEvent.actor.mbox = userData.UserEmail;


        xapiEvent.@object.id = moodleUrl + "/xapi/activity/" + contextId;

        // serialize the XAPI Event again
        var inText = JsonSerializer.Serialize(xapiEvent);

        // Send the XAPI Event to the LMS
        var isSuccess = await _ilms.ProcessXApiViaPlugin(request.WebServiceToken, inText);

        return new ScoreElementResponse
        {
            IsSuccess = isSuccess
        };
    }
}
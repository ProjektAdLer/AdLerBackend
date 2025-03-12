using System.Text.Json;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreH5PStrategy;

public class ScoreH5PElementStrategyHandler(ISerialization serialization, ILMS lms, IOptions<BackendConfig> config)
    : IRequestHandler<ScoreH5PElementStrategyCommand, ScoreElementResponse>
{
    private readonly BackendConfig _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
    private readonly ILMS _lms = lms ?? throw new ArgumentNullException(nameof(lms));

    private readonly ISerialization _serialization =
        serialization ?? throw new ArgumentNullException(nameof(serialization));

    public async Task<ScoreElementResponse> Handle(ScoreH5PElementStrategyCommand request,
        CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var moodleUrl = GetSanitizedMoodleUrl();
        var userData = await _lms.GetLMSUserDataAsync(request.WebServiceToken);
        var xapiEvent = DeserializeAndUpdateXapiEvent(request, userData, moodleUrl);

        LogXapiEvent(xapiEvent);

        var elementViewedScore = await _lms.ScoreGenericElementViaPlugin(request.WebServiceToken, request.LmsModule.Id);
        var isSuccess = await _lms.ProcessXApiViaPlugin(request.WebServiceToken, JsonSerializer.Serialize(xapiEvent));

        return new ScoreElementResponse {IsSuccess = isSuccess};
    }

    private string GetSanitizedMoodleUrl()
    {
        if (string.IsNullOrEmpty(_config.MoodleUrl))
            throw new InvalidOperationException("Moodle Host is not set in the configuration file");

        return _config.MoodleUrl.TrimEnd('/');
    }

    private RawH5PEvent DeserializeAndUpdateXapiEvent(ScoreH5PElementStrategyCommand request,
        LMSUserDataResponse userData, string moodleUrl)
    {
        var xapiEvent = _serialization.GetObjectFromJsonString<RawH5PEvent>(
            request.ScoreElementParams.SerializedXapiEvent
            ?? throw new ArgumentException("SerializedXapiEvent is null", nameof(request)));

        xapiEvent.actor.name = userData.LMSUserName;
        xapiEvent.actor.mbox = userData.UserEmail;
        xapiEvent.@object.id = $"{moodleUrl}/xapi/activity/{request.LmsModule.contextid}";

        return xapiEvent;
    }

    private void LogXapiEvent(RawH5PEvent xapiEvent)
    {
        Console.WriteLine(JsonSerializer.Serialize(xapiEvent));
    }
}
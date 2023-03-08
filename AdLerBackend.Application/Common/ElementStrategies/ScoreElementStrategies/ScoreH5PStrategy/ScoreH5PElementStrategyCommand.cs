using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;

#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreH5PStrategy;

public record ScoreH5PElementStrategyCommand : CommandWithToken<ScoreElementResponse>
{
    public Modules Module { get; init; }
    public ScoreElementParams ScoreElementParams { get; init; }
}
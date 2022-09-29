using AdLerBackend.Application.Common.Interfaces;
using FluentValidation;

namespace AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.ScoreH5PStrategy;

public class ScoreH5PElementStrategyValidator : AbstractValidator<ScoreH5PElementStrategyCommand>
{
    private readonly ISerialization _serialization;

    public ScoreH5PElementStrategyValidator(ISerialization serialization)
    {
        _serialization = serialization;
        // Has to be a Valid JSON
        RuleFor(x => x.ScoreElementParams.SerializedXapiEvent).Must(x => _serialization.IsValidJsonString(x))
            .WithMessage("Content is not a valid JSON");
    }
}
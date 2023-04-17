using AdLerBackend.Application.Common.Interfaces;
using FluentValidation;

namespace AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreH5PStrategy;

public class ScoreH5PElementStrategyValidator : AbstractValidator<ScoreH5PElementStrategyCommand>
{
    public ScoreH5PElementStrategyValidator(ISerialization serialization)
    {
        // Has to be a Valid JSON
        RuleFor(x => x.ScoreElementParams.SerializedXapiEvent!).Must(serialization.IsValidJsonString)
            .WithMessage("Content is not a valid JSON");
    }
}
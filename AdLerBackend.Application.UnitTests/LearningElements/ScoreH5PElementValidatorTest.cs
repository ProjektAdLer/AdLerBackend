using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.ScoreH5PStrategy;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class ScoreH5PElementValidatorTest
{
    private ISerialization _serialization;

    [TestCase]
    public void Should_HaveError_InvalidJson()
    {
        _serialization = Substitute.For<ISerialization>();
        var validator = new ScoreH5PElementStrategyValidator(_serialization);
        var command = new ScoreH5PElementStrategyCommand
        {
            ScoreElementParams = new ScoreElementParams
            {
                SerializedXapiEvent = "string"
            }
        };

        var result = validator.TestValidate(command);

        result.IsValid.Should().Be(false);
    }
}
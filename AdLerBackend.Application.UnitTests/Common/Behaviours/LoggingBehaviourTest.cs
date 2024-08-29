using AdLerBackend.Application.Common.Behaviours;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Common.Behaviours;

public class LoggingBehaviourTest
{
    private ILogger<LoggingBehavior<object, string>> _mockedLogger;

    [SetUp]
    public void SetUp()
    {
        _mockedLogger = Substitute.For<ILogger<LoggingBehavior<object, string>>>();
    }

    [Test]
    public async Task Handle_Valid_ReturnsResponse()
    {
        // Arrange
        var systemUnderTest = new LoggingBehavior<object, string>(_mockedLogger);

        // Act
        var result =
            await systemUnderTest.Handle(new object(), () => Task.FromResult("meinTest"), new CancellationToken());

        // Assert
        result.Should().Be("meinTest");


        // Verify START log
        _mockedLogger.Received(1).Log(
            Arg.Is(LogLevel.Trace),
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString().Contains("[START]")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());

        // Verify END log
        _mockedLogger.Received(1).Log(
            Arg.Is(LogLevel.Trace),
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString().Contains("[END]") && o.ToString().Contains("Execution time=")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>());
    }
}
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.MockPrimitiveH5PStrategy;
using FluentAssertions;

namespace AdLerBackend.Application.UnitTests.Common.ElementStrategies
{
    public class MockPrimitiveH5PStrategyCommandHandlerTests
    {
        private MockPrimitiveH5PStrategyCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new MockPrimitiveH5PStrategyCommandHandler();
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsSuccessResponse()
        {
            // Arrange
            var command = new MockPrimitiveH5PStrategyCommand
            {
                ElementId = 1
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            
            // Assert
            result.IsSuccess.Should().BeTrue();
        }
    }
}
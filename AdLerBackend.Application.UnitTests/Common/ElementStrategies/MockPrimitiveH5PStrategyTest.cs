using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.MockPrimitiveH5PStrategy;
using AutoBogus;
using FluentAssertions;
using Force.DeepCloner;

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
        
        [TestCaseSource(nameof(GetTestCases))]
        public void PlayerDataResponseGetterAndSetter<T>(T _)
        {
            // Arrange
            var testClass = AutoFaker.Generate<T>();

            // Recursively clone the object
            var clone = testClass.DeepClone();

            // Assert
            clone.Should().BeEquivalentTo(testClass);
        }
        
        private static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(new MockPrimitiveH5PStrategyCommand());

        }
    }
}
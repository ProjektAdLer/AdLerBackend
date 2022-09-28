using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.Player;
using AutoBogus;
using FluentAssertions;
using Force.DeepCloner;

namespace AdLerBackend.Application.UnitTests.Common.Responses;

public class AllResponsesTest
{
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
        yield return new TestCaseData(new PlayerDataResponse());
        yield return new TestCaseData(new LearningElementStatusResponse());
        yield return new TestCaseData(new LearningElementScoreResponse());
        yield return new TestCaseData(new CourseResponse());
        yield return new TestCaseData(new LearningWorld());
        yield return new TestCaseData(new PlayerDataResponse());
        yield return new TestCaseData(new ScoreLearningElementResponse());
        yield return new TestCaseData(new CourseContent());
    }
}
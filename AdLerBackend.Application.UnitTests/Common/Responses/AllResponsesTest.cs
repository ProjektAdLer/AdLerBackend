using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.Player;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.Configuration;
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
        yield return new TestCaseData(new WorldStatusResponse());
        yield return new TestCaseData(new ElementScoreResponse());
        yield return new TestCaseData(new WorldResponse());
        yield return new TestCaseData(new Application.Common.Responses.World.World());
        yield return new TestCaseData(new PlayerDataResponse());
        yield return new TestCaseData(new ScoreElementResponse());
        yield return new TestCaseData(new LMSWorldContentResponse());
        yield return new TestCaseData(new WorldAtfResponse());
        yield return new TestCaseData(new BackendConfig());
        yield return new TestCaseData(new WorldBaseStorageDto());
    }
}
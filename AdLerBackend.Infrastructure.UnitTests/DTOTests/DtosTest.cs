using AdLerBackend.Infrastructure.Moodle.ApiResponses;
using AutoBogus;
using FluentAssertions;
using Force.DeepCloner;
using File = AdLerBackend.Infrastructure.LmsBackup.File;

namespace AdLerBackend.Infrastructure.UnitTests.DTOTests;

public class DtosTest
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
        yield return new TestCaseData(new PluginUUIDResponse());
        yield return new TestCaseData(new DetailedUserDataResponse());
        yield return new TestCaseData(new GeneralUserDataResponse());
        yield return new TestCaseData(new MoodleWsErrorResponse());
        yield return new TestCaseData(new PluginCourseCreationResponse());
        yield return new TestCaseData(new ResponseWithData<PluginUUIDResponse>());
        yield return new TestCaseData(new ResponseWithDataArray<PluginUUIDResponse>());
        yield return new TestCaseData(new PluginElementScoreData());
        yield return new TestCaseData(new ScoreGenericLearningElementResponse());
        yield return new TestCaseData(new UserTokenResponse());
        yield return new TestCaseData(new File());
    }
}
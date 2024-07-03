using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.MockPrimitiveH5PStrategy;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.Adaptivity;
using AdLerBackend.Application.Common.Responses.Adaptivity.Common;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;
using AdLerBackend.Application.Common.Responses.World;
using AutoBogus;
using FluentAssertions;
using Force.DeepCloner;

namespace AdLerBackend.Application.UnitTests;

public class PocoTest
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
        yield return new TestCaseData(new MockPrimitiveH5PStrategyCommand());
        yield return new TestCaseData(new MoodleCourse());
        yield return new TestCaseData(new LmsUuidResponse());
        yield return new TestCaseData(new AnswerAdaptivityQuestionResponse()
        {
            ElementScore = new ElementScoreResponse(),
            GradedTask = new GradedTask(),
            GradedQuestion = new GradedQuestion()
        });
        yield return new TestCaseData(new GetAdaptivityElementDetailsResponse());
        yield return new TestCaseData(new LMSAdaptivityQuestionStateResponse());
        yield return new TestCaseData(new AdaptivityModuleStateResponseAfterAnswer());
        yield return new TestCaseData(new CreateWorldResponse());
        yield return new TestCaseData(new GetAllElementsFromLmsWithAdLerIdResponse());
        yield return new TestCaseData(new Application.Common.Responses.World.Element());
        yield return new TestCaseData(new AdaptivityContent());
        yield return new TestCaseData(new AdaptivityTask());
        yield return new TestCaseData(new AdaptivityQuestion());
        yield return new TestCaseData(new CommentAction());
        yield return new TestCaseData(new ElementReferenceAction());
        yield return new TestCaseData(new AdaptivityTrigger());
        yield return new TestCaseData(new AdaptivityQuestionAnswer());
        yield return new TestCaseData(new ContentReferenceAction());
        yield return new TestCaseData(new GetLearningElementCommand());
        yield return new TestCaseData(new LmsUuidResponse()
        {
            LmsContextId = 1,
        });
    }
}
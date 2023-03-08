using AdLerBackend.Domain.Entities;
using AutoBogus;
using FluentAssertions;
using Force.DeepCloner;

namespace AdLerBackend.Domain.UnitTests.Entities;

public class CourseEntityTest
{
    [TestCaseSource(nameof(GetTestCases))]
    public void CourseEntityGetterAndSetter<T>(T _)
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
        yield return new TestCaseData(new WorldEntity());
    }
}
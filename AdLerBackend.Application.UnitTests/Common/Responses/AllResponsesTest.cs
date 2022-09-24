using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AutoBogus;
using FluentAssertions;
using Force.DeepCloner;

namespace AdLerBackend.Application.UnitTests.Common.Responses;

public class AllResponsesTest
{
    [Test]
    public async Task CourseContentGetterAndSetter()
    {
        // Arrange
        var course = AutoFaker.Generate<CourseContent>();

        // Recursively clone the object
        var clone = course.DeepClone();

        // Assert
        clone.Should().BeEquivalentTo(course);
    }

    [Test]
    public async Task ScoreLearningElementResponseGetterAndSetter()
    {
        // Arrange
        var course = AutoFaker.Generate<ScoreLearningElementResponse>();

        // Recursively clone the object
        var clone = course.DeepClone();

        // Assert
        clone.Should().BeEquivalentTo(course);
    }


    [Test]
    public async Task LearningWorldGetterAndSetter()
    {
        // Arrange
        var course = AutoFaker.Generate<LearningWorld>();

        // Recursively clone the object
        var clone = course.DeepClone();

        // Assert
        clone.Should().BeEquivalentTo(course);
    }

    [Test]
    public async Task CourseResponseGetterAndSetter()
    {
        // Arrange
        var course = AutoFaker.Generate<CourseResponse>();

        // Recursively clone the object
        var clone = course.DeepClone();

        // Assert
        clone.Should().BeEquivalentTo(course);
    }
}
using AdLerBackend.Domain.Entities;
using AutoBogus;
using FluentAssertions;
using Force.DeepCloner;

namespace AdLerBackend.Domain.UnitTests.Entities;

public class CourseEntityTest
{
    [Test]
    public async Task CourseEntityGetterAndSetter()
    {
        // Arrange
        var course = AutoFaker.Generate<CourseEntity>();

        // Recursively clone the object
        var clone = course.DeepClone();

        // Assert
        clone.Should().BeEquivalentTo(course);
    }
}
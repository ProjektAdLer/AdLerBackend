using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.LearningElement.H5P.GetH5PFilePath;
using AdLerBackend.Domain.Entities;
using AutoBogus;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.LearningElements.H5P;

public class GetH5PFilePathTest
{
    private ICourseRepository _courseRepository;

    [SetUp]
    public void Setup()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
    }

    [Test]
    public async Task GetH5PFilePath_WhenCalled_ReturnsH5PFilePath()
    {
        // Arrange
        var systemUnderTest = new GetH5PFilePathHandler(_courseRepository);
        var courseMock = AutoFaker.Generate<CourseEntity>();

        courseMock.Id = 1;
        courseMock.H5PFilesInCourse = new List<H5PLocationEntity>
        {
            new()
            {
                Id = 2,
                ElementId = 2,
                Path = "path"
            }
        };

        _courseRepository.GetAsync(Arg.Any<int>()).Returns(courseMock);

        // Act
        var result = await systemUnderTest.Handle(new GetH5PFilePathCommand
        {
            CourseId = 1,
            ElementId = 2,
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        Assert.That(result.FilePath, Is.EqualTo("path"));
    }

    [Test]
    public async Task GetH5PFilePath_CourseNotFound_Throws()
    {
        // Arrange
        var systemUnderTest = new GetH5PFilePathHandler(_courseRepository);


        _courseRepository.GetAsync(Arg.Any<int>()).Returns((CourseEntity) null);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await systemUnderTest.Handle(new GetH5PFilePathCommand
        {
            CourseId = 1,
            ElementId = 2,
            WebServiceToken = "token"
        }, CancellationToken.None));
    }

    [Test]
    public async Task GetH5PFilePath_H5PFileNotFound_Throws()
    {
        // Arrange
        var systemUnderTest = new GetH5PFilePathHandler(_courseRepository);


        var courseMock = AutoFaker.Generate<CourseEntity>();

        courseMock.Id = 1;
        courseMock.H5PFilesInCourse = new List<H5PLocationEntity>
        {
            new()
            {
                Id = 24,
                ElementId = 23,
                Path = "path"
            }
        };

        _courseRepository.GetAsync(Arg.Any<int>()).Returns(courseMock);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await systemUnderTest.Handle(new GetH5PFilePathCommand
        {
            CourseId = 1,
            ElementId = 2,
            WebServiceToken = "token"
        }, CancellationToken.None));
    }
}
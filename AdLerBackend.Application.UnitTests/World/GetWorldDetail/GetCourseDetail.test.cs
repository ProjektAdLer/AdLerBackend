using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.World.GetWorldDetail;
using AdLerBackend.Domain.Entities;
using AutoBogus;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.World.GetWorldDetail;

public class GetWorldDetailTest
{
    private IFileAccess _fileAccess;
    private ISerialization _serialization;
    private IWorldRepository _worldRepository;

    [SetUp]
    public void Setup()
    {
        _worldRepository = Substitute.For<IWorldRepository>();
        _fileAccess = Substitute.For<IFileAccess>();
        _serialization = Substitute.For<ISerialization>();
    }

    [Test]
    public async Task Handle_GivenValidId_ReturnsCourseDetail()
    {
        // Arrange
        var request = new GetWorldDetailCommand
        {
            WorldId = 1,
            WebServiceToken = "testToken"
        };

        var courseDatabaseResponse = new WorldEntity
        {
            Id = 1,
            H5PFilesInCourse = new List<H5PLocationEntity>
            {
                new()
                {
                    Path = Path.Combine("some", "path1")
                },
                new()
                {
                    Path = Path.Combine("some", "path2")
                }
            }
        };

        _worldRepository.GetAsync(Arg.Any<int>()).Returns(courseDatabaseResponse);

        var stream = new MemoryStream();
        _fileAccess.GetFileStream(Arg.Any<string>()).Returns(stream);

        var mockedDsl = AutoFaker.Generate<WorldDtoResponse>();
        mockedDsl.LearningWorld.LearningElements = new List<LearningElement>
        {
            new()
            {
                Id = 1,
                ElementCategory = "h5p",
                Identifier = new Identifier
                {
                    Value = "path1"
                }
            },
            new()
            {
                Id = 2,
                ElementCategory = "h5p",
                Identifier = new Identifier
                {
                    Value = "path2"
                }
            }
        };

        _serialization.GetObjectFromJsonStreamAsync<WorldDtoResponse>(Arg.Any<Stream>())
            .Returns(mockedDsl);

        var systemUnderTest = new GetWorldDetailHandler(_worldRepository, _fileAccess, _serialization);

        // Act
        var result = await systemUnderTest.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.That(result.LearningWorld.LearningElements.Count, Is.EqualTo(2));
    }

    [Test]
    public Task Handle_CourseNotFound_ThrowsNotFound()
    {
        // Arrange
        var request = new GetWorldDetailCommand
        {
            WorldId = 1,
            WebServiceToken = "testToken"
        };

        WorldEntity? courseDatabaseResponse = null;

        _worldRepository.GetAsync(Arg.Any<int>()).Returns(courseDatabaseResponse);

        var systemUnderTest = new GetWorldDetailHandler(_worldRepository, _fileAccess, _serialization);

        // Act
        Assert.ThrowsAsync<NotFoundException>(async () =>
            await systemUnderTest.Handle(request, CancellationToken.None));
        return Task.CompletedTask;
    }

    // [Test]
    // public Task Handle_CourseNotFoundInMoodle_ThrowsNotFound()
    // {
    //     // Arrange
    //     var request = new GetCourseDetailCommand
    //     {
    //         CourseId = 1,
    //         WebServiceToken = "testToken"
    //     };
    //
    //     var courseDatabaseResponse = new CourseEntity
    //     {
    //         Id = 1,
    //         H5PFilesInCourse = new List<H5PLocationEntity>
    //         {
    //             new()
    //             {
    //                 Path = Path.Combine("some", "path1")
    //             },
    //             new()
    //             {
    //                 Path = Path.Combine("some", "path2")
    //             }
    //         }
    //     };
    //
    //     _courseRepository.GetAsync(Arg.Any<int>()).Returns(courseDatabaseResponse);
    //
    //     var systemUnderTest = new GetCourseDetailHandler(_moodle, _courseRepository, _fileAccess, _serialization);
    //
    //     var moodleCourseResponse = new MoodleCourseListResponse
    //     {
    //         Total = 0
    //     };
    //
    //     _moodle.SearchCoursesAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(moodleCourseResponse);
    //
    //
    //     // Act
    //     Assert.ThrowsAsync<NotFoundException>(async () =>
    //         await systemUnderTest.Handle(request, CancellationToken.None));
    //     return Task.CompletedTask;
    // }

    // [Test]
    // public Task Handle_H5PFilesNotFound_ThrowsNotFoundException()
    // {
    //     // Arrange
    //     var request = new GetCourseDetailCommand
    //     {
    //         CourseId = 1,
    //         WebServiceToken = "testToken"
    //     };
    //
    //     var courseDatabaseResponse = new CourseEntity
    //     {
    //         Id = 1,
    //         H5PFilesInCourse = new List<H5PLocationEntity>
    //         {
    //             new()
    //             {
    //                 Path = Path.Combine("some", "path1")
    //             },
    //             new()
    //             {
    //                 Path = Path.Combine("some", "path2")
    //             }
    //         }
    //     };
    //
    //     _courseRepository.GetAsync(Arg.Any<int>()).Returns(courseDatabaseResponse);
    //
    //     var moodleCourseResponse = new MoodleCourseListResponse
    //     {
    //         Total = 1,
    //         Courses = new List<MoodleCourse>
    //         {
    //             new()
    //             {
    //                 Id = 1
    //             }
    //         }
    //     };
    //
    //     _moodle.SearchCoursesAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(moodleCourseResponse);
    //
    //     // TODO: Is this the right way to mock the stream?
    //     var stream = new MemoryStream();
    //     _fileAccess.GetFileStream(Arg.Any<string>()).Returns(stream);
    //
    //     var mockedDsl = AutoFaker.Generate<LearningWorldDtoResponse>();
    //     mockedDsl.LearningWorld.LearningElements = new List<Application.Common.Responses.Course.LearningElement>
    //     {
    //         new()
    //         {
    //             Id = 1,
    //             ElementType = "h5p",
    //             Identifier = new Identifier
    //             {
    //                 Value = "path1FOO"
    //             }
    //         },
    //         new()
    //         {
    //             Id = 2,
    //             ElementType = "h5p",
    //             Identifier = new Identifier
    //             {
    //                 Value = "path2"
    //             }
    //         }
    //     };
    //
    //     _serialization.GetObjectFromJsonStreamAsync<LearningWorldDtoResponse>(Arg.Any<Stream>())
    //         .Returns(mockedDsl);
    //
    //     var systemUnderTest = new GetCourseDetailHandler(_moodle, _courseRepository, _fileAccess, _serialization);
    //
    //     // Act
    //     Assert.ThrowsAsync<NotFoundException>(async () =>
    //         await systemUnderTest.Handle(request, CancellationToken.None));
    //     return Task.CompletedTask;
    // }
}
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LearningElement.H5P.ScoreH5PElement;
using AdLerBackend.Domain.Entities;
using AutoBogus;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class ScoreLearningElement
{
    private ICourseRepository _courseRepository;
    private IFileAccess _fileAccess;
    private IMediator _mediator;
    private IMoodle _moodle;
    private ISerialization _serialization;

    [SetUp]
    public void Setup()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
        _fileAccess = Substitute.For<IFileAccess>();
        _mediator = Substitute.For<IMediator>();
        _moodle = Substitute.For<IMoodle>();
        _serialization = Substitute.For<ISerialization>();
    }

    [Test]
    public async Task ScoreH5PElement_Valid_CallsWebservices()
    {
        // Arrange

        _moodle.GetMoodleUserDataAsync(Arg.Any<string>()).Returns(new MoodleUserDataResponse
        {
            IsAdmin = false,
            UserEmail = "email",
            UserId = 1,
            MoodleUserName = "moodleUserName"
        });

        _courseRepository.GetAsync(Arg.Any<int>()).Returns(new CourseEntity
        {
            Id = 2,
            Name = "name",
            AuthorId = 1234,
            DslLocation = "asd",
            H5PFilesInCourse = new List<H5PLocationEntity>
            {
                new()
                {
                    Id = 3,
                    Path = "path",
                    ElementId = 4
                }
            }
        });

        _fileAccess.GetFileStream(Arg.Any<string>()).Returns(new MemoryStream());
        _serialization.GetObjectFromJsonStreamAsync<LearningWorldDtoResponse>(Arg.Any<Stream>())
            .Returns(new LearningWorldDtoResponse
            {
                LearningWorld = new LearningWorld
                {
                    LearningElements = new List<Application.Common.Responses.Course.LearningElement>
                    {
                        new()
                        {
                            Id = 1337,
                            Identifier = new Identifier
                            {
                                Value = "searchedFileName"
                            }
                        }
                    }
                }
            });

        _moodle.SearchCoursesAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(new MoodleCourseListResponse
        {
            Courses = new List<MoodleCourse>
            {
                new()
                {
                    Id = 1
                }
            }
        });

        _moodle.GetCourseContentAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(new[]
        {
            new CourseContent
            {
                Modules = new List<Modules>
                {
                    new()
                    {
                        Name = "searchedFileName",
                        contextid = 123
                    }
                }
            }
        });

        var fakeXAPI = AutoFaker.Generate<RawH5PEvent>();

        _serialization.GetObjectFromJsonString<RawH5PEvent>(Arg.Any<string>()).Returns(fakeXAPI);


        var systemUnderTest =
            new ScoreH5PElementHandler(_serialization, _moodle, _courseRepository, _fileAccess, _mediator);

        // Act
        await systemUnderTest.Handle(new ScoreElementCommand
        {
            CourseId = 1,
            ElementId = 1337,
            ScoreElementParams = new ScoreElementParams
            {
                SerializedXapiEvent = "xapiEvent"
            },
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        await _moodle.Received(1).GetMoodleUserDataAsync(Arg.Any<string>());
    }


    [Test]
    public async Task ScoreH5PElement_H5PFileNotExistent_Throws()
    {
        // Arrange

        _moodle.GetMoodleUserDataAsync(Arg.Any<string>()).Returns(new MoodleUserDataResponse
        {
            IsAdmin = false,
            UserEmail = "email",
            UserId = 1,
            MoodleUserName = "moodleUserName"
        });

        _courseRepository.GetAsync(Arg.Any<int>()).Returns((CourseEntity?) null);

        _fileAccess.GetFileStream(Arg.Any<string>()).Returns(new MemoryStream());
        _serialization.GetObjectFromJsonStreamAsync<LearningWorldDtoResponse>(Arg.Any<Stream>())
            .Returns(new LearningWorldDtoResponse
            {
                LearningWorld = new LearningWorld
                {
                    LearningElements = new List<Application.Common.Responses.Course.LearningElement>
                    {
                        new()
                        {
                            Id = 1337,
                            Identifier = new Identifier
                            {
                                Value = "searchedFileName123"
                            }
                        }
                    }
                }
            });

        _moodle.SearchCoursesAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(new MoodleCourseListResponse
        {
            Courses = new List<MoodleCourse>
            {
                new()
                {
                    Id = 1
                }
            }
        });

        _moodle.GetCourseContentAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(new[]
        {
            new CourseContent
            {
                Modules = new List<Modules>
                {
                    new()
                    {
                        Name = "searchedFileName",
                        contextid = 123
                    }
                }
            }
        });

        var fakeXAPI = AutoFaker.Generate<RawH5PEvent>();

        _serialization.GetObjectFromJsonString<RawH5PEvent>(Arg.Any<string>()).Returns(fakeXAPI);


        var systemUnderTest =
            new ScoreH5PElementHandler(_serialization, _moodle, _courseRepository, _fileAccess, _mediator);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () =>
            await systemUnderTest.Handle(new ScoreElementCommand
            {
                CourseId = 1,
                ElementId = 1337,
                ScoreElementParams = new ScoreElementParams
                {
                    SerializedXapiEvent = "xapiEvent"
                },
                WebServiceToken = "token"
            }, CancellationToken.None));
    }

    [Test]
    public async Task ScoreH5PElement_CourseNotExistent_Throws()
    {
        // Arrange

        _moodle.GetMoodleUserDataAsync(Arg.Any<string>()).Returns(new MoodleUserDataResponse
        {
            IsAdmin = false,
            UserEmail = "email",
            UserId = 1,
            MoodleUserName = "moodleUserName"
        });

        _courseRepository.GetAsync(Arg.Any<int>()).Returns(new CourseEntity
        {
            Id = 2,
            Name = "name",
            AuthorId = 1234,
            DslLocation = "asd",
            H5PFilesInCourse = new List<H5PLocationEntity>
            {
                new()
                {
                    Id = 3,
                    Path = "path",
                    ElementId = 4
                }
            }
        });

        _fileAccess.GetFileStream(Arg.Any<string>()).Returns(new MemoryStream());
        _serialization.GetObjectFromJsonStreamAsync<LearningWorldDtoResponse>(Arg.Any<Stream>())
            .Returns(new LearningWorldDtoResponse
            {
                LearningWorld = new LearningWorld
                {
                    LearningElements = new List<Application.Common.Responses.Course.LearningElement>
                    {
                        new()
                        {
                            Id = 13345437,
                            Identifier = new Identifier
                            {
                                Value = "searchedFileName123"
                            }
                        }
                    }
                }
            });

        _moodle.SearchCoursesAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(new MoodleCourseListResponse
        {
            Courses = new List<MoodleCourse>
            {
                new()
                {
                    Id = 1
                }
            }
        });

        _moodle.GetCourseContentAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(new[]
        {
            new CourseContent
            {
                Modules = new List<Modules>
                {
                    new()
                    {
                        Name = "searchedFileName",
                        contextid = 123
                    }
                }
            }
        });

        var fakeXAPI = AutoFaker.Generate<RawH5PEvent>();

        _serialization.GetObjectFromJsonString<RawH5PEvent>(Arg.Any<string>()).Returns(fakeXAPI);


        var systemUnderTest =
            new ScoreH5PElementHandler(_serialization, _moodle, _courseRepository, _fileAccess, _mediator);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () =>
            await systemUnderTest.Handle(new ScoreElementCommand
            {
                CourseId = 100,
                ElementId = 1337,
                ScoreElementParams = new ScoreElementParams
                {
                    SerializedXapiEvent = "xapiEvent"
                },
                WebServiceToken = "token"
            }, CancellationToken.None));
    }
}
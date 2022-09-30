using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Course.CourseManagement.DeleteCourse;
using AdLerBackend.Application.Moodle.GetUserData;
using AdLerBackend.Domain.Entities;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.Course.CourseManagement.DeleteCourse;

public class DeleteCourseTest
{
    private ICourseRepository _courseRepository;
    private IFileAccess _fileAccess;
    private IMediator _mediator;


    [SetUp]
    public void Setup()
    {
        _courseRepository = Substitute.For<ICourseRepository>();
        _fileAccess = Substitute.For<IFileAccess>();
        _mediator = Substitute.For<IMediator>();
    }

    [Test]
    public async Task Handle_Valid_ShouldCallDeletionOfCourse()
    {
        // Arrange
        var systemUnderTest = new DeleteCourseHandler(_courseRepository, _fileAccess, _mediator);

        var courseMock = new CourseEntity
        {
            Id = 1,
            AuthorId = 1
        };

        _mediator.Send(Arg.Any<GetMoodleUserDataCommand>()).Returns(new MoodleUserDataResponse
        {
            UserId = 1
        });

        _courseRepository.GetAsync(Arg.Any<int>()).Returns(courseMock);

        _fileAccess.DeleteCourse(Arg.Any<CourseDeleteDto>()).Returns(true);

        // Act
        var result = await systemUnderTest.Handle(new DeleteCourseCommand
        {
            CourseId = 1,
            WebServiceToken = "testToken"
        }, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
        // Expect DeleteCourse to be called once
        _fileAccess.Received(1).DeleteCourse(Arg.Any<CourseDeleteDto>());

        // Assert that DeleteAsync was called
        await _courseRepository.Received(1).DeleteAsync(Arg.Any<int>());
    }

    [Test]
    public async Task Handle_UserNotAdmin_ShouldThorwException()
    {
        // Arrange
        var systemUnderTest = new DeleteCourseHandler(_courseRepository, _fileAccess, _mediator);
        _mediator.Send(Arg.Any<CheckUserPrivilegesCommand>()).Throws(new ForbiddenAccessException(""));

        // Act
        // Assert
        Assert.ThrowsAsync<ForbiddenAccessException>(async () => await systemUnderTest.Handle(new DeleteCourseCommand
        {
            CourseId = 1,
            WebServiceToken = "testToken"
        }, CancellationToken.None));
    }

    [Test]
    public async Task Handle_CourseNotExistent_ShouldThorwException()
    {
        // Arrange
        var systemUnderTest = new DeleteCourseHandler(_courseRepository, _fileAccess, _mediator);

        _courseRepository.GetAsync(Arg.Any<int>()).Returns((CourseEntity?) null);

        _fileAccess.DeleteCourse(Arg.Any<CourseDeleteDto>()).Returns(true);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await systemUnderTest.Handle(new DeleteCourseCommand
        {
            CourseId = 1,
            WebServiceToken = "testToken"
        }, CancellationToken.None));
    }

    [Test]
    public async Task Handle_CourseNotFromSameAuthor_ShouldThorwException()
    {
        // Arrange
        var systemUnderTest = new DeleteCourseHandler(_courseRepository, _fileAccess, _mediator);

        var courseMock = new CourseEntity
        {
            Id = 1,
            AuthorId = 1337
        };

        _mediator.Send(Arg.Any<GetMoodleUserDataCommand>()).Returns(new MoodleUserDataResponse
        {
            UserId = 1
        });

        _courseRepository.GetAsync(Arg.Any<int>()).Returns(courseMock);

        _fileAccess.DeleteCourse(Arg.Any<CourseDeleteDto>()).Returns(true);

        // Act
        // Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await systemUnderTest.Handle(new DeleteCourseCommand
        {
            CourseId = 1,
            WebServiceToken = "testToken"
        }, CancellationToken.None));
    }
}
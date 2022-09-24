using AdLerBackend.Application.Common.DTOs.Storage;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using AdLerBackend.Application.Course.CourseManagement.DeleteCourse;
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

        _mediator.Send(Arg.Any<CheckUserPrivilegesCommand>()).Returns(Unit.Task);

        var courseMock = new CourseEntity
        {
            Id = 1
        };

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
    public Task Handle_UserNotAdmin_ShouldThorwException()
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
        return Task.CompletedTask;
    }
}
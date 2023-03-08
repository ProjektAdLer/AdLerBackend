using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using AdLerBackend.Application.World.WorldManagement.UploadH5pBase;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.World.WorldManagement.UploadH5PBase;

public class UploadH5PBaseTest
{
    private IFileAccess _fileAccess;
    private IMediator _mediator;
    private UploadH5PBaseHandler _systemUnderTest;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
        _fileAccess = Substitute.For<IFileAccess>();
        _systemUnderTest = new UploadH5PBaseHandler(_fileAccess, _mediator);
    }

    [Test]
    public void Handle_UserNotAuthorized_Throws()
    {
        // Arrange
        _mediator.Send(Arg.Any<CheckUserPrivilegesCommand>()).Throws(new ForbiddenAccessException(""));

        // Act
        // Assert
        Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            _systemUnderTest.Handle(new UploadH5PBaseCommand(), CancellationToken.None));
    }

    [Test]
    public async Task Handle_Valud_ShouldCallFileStorage()
    {
        // Arrange
        _mediator.Send(Arg.Any<CheckUserPrivilegesCommand>()).Returns(Unit.Task);
        // Act
        await _systemUnderTest.Handle(new UploadH5PBaseCommand(), CancellationToken.None);

        // Assert
        _fileAccess.Received(1).StoreH5PBase(Arg.Any<Stream>());
    }
}
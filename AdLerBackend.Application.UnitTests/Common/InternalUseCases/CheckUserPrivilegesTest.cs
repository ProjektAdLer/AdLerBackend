using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Common.InternalUseCases;

public class CheckUserPrivilegesTest
{
    private IMoodle _moodle;

    [SetUp]
    public void Setup()
    {
        _moodle = Substitute.For<IMoodle>();
    }

    [Test]
    public async Task CheckUserPrivileges_WhenUserIsNotAdmin_ThrowsException()
    {
        // Arrange
        _moodle.IsMoodleAdminAsync(Arg.Any<string>()).Returns(false);
        var systemUnderTest = new CheckUserPrivilegesHandler(_moodle);

        // Act
        // Assert
        Assert.ThrowsAsync<ForbiddenAccessException>(async () =>
            await systemUnderTest.Handle(new CheckUserPrivilegesCommand
            {
                Role = UserRoles.Admin,
                WebServiceToken = "testToken"
            }, CancellationToken.None));
    }

    [Test]
    public async Task CheckUserPrivileges_ReturnsUnitValue()
    {
        // Arrange
        _moodle.IsMoodleAdminAsync(Arg.Any<string>()).Returns(true);
        var systemUnderTest = new CheckUserPrivilegesHandler(_moodle);

        // Act
        var ret = await systemUnderTest.Handle(new CheckUserPrivilegesCommand
        {
            Role = UserRoles.Admin,
            WebServiceToken = "testToken"
        }, CancellationToken.None);

        // Assert
        Assert.AreEqual(Unit.Value, ret);
    }
}
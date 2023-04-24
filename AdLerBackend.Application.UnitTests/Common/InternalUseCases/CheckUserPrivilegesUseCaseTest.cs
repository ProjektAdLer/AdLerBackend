using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.CheckUserPrivileges;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Common.InternalUseCases;

public class CheckUserPrivilegesUseCaseTest
{
    private ILMS _ilms;

    [SetUp]
    public void Setup()
    {
        _ilms = Substitute.For<ILMS>();
    }

    [Test]
    public async Task CheckUserPrivileges_WhenUserIsNotAdmin_ThrowsException()
    {
        // Arrange
        _ilms.IsLMSAdminAsync(Arg.Any<string>()).Returns(false);
        var systemUnderTest = new CheckUserPrivilegesUseCase(_ilms);

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
        _ilms.IsLMSAdminAsync(Arg.Any<string>()).Returns(true);
        var systemUnderTest = new CheckUserPrivilegesUseCase(_ilms);

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
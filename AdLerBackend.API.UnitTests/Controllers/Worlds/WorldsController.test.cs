using AdLerBackend.API.Controllers.Worlds;
using AdLerBackend.Application.World.GetElementStatus;
using AdLerBackend.Application.World.GetWorldDetail;
using AdLerBackend.Application.World.GetWorldsForAuthor;
using AdLerBackend.Application.World.GetWorldsForUser;
using AdLerBackend.Application.World.WorldManagement.DeleteWorld;
using AdLerBackend.Application.World.WorldManagement.UploadWorld;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Controllers.Worlds;

public class WorldsControllerTest
{
    [Test]
    public async Task GetWorldsForAuthor_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new WorldsController(mediatorMock);

        // Act
        await controller.GetWorldsForAuthor("token", 1337);

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetWorldsForAuthorCommand>(x => x.WebServiceToken == "token" && x.AuthorId == 1337));
    }

    [Test]
    public async Task GetWorldsForUser_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new WorldsController(mediatorMock);

        // Act
        await controller.GetWorldsForUser("token");

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetWorldsForUserCommand>(x => x.WebServiceToken == "token"));
    }

    [Test]
    public async Task CreateWorld_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new WorldsController(mediatorMock);
        var backupFile = Substitute.For<IFormFile>();
        var dslFile = Substitute.For<IFormFile>();

        // Act
        await controller.CreateWorld(backupFile, dslFile, "token");

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<UploadWorldCommand>(x => x.WebServiceToken == "token"));
    }


    [Test]
    public async Task DeleteWorld_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new WorldsController(mediatorMock);

        // Act
        await controller.DeleteWorld("token", 1337);

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<DeleteWorldCommand>(x => x.WebServiceToken == "token"));
    }

    [Test]
    public async Task GetWorldATF_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new WorldsController(mediatorMock);

        // Act
        await controller.GetWorldAtf("token", 1337);

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetWorldDetailCommand>(x => x.WebServiceToken == "token"));
    }

    [Test]
    public async Task GetElementStatus_ShouldForwardCallToMediator()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new WorldsController(mediatorMock);

        // Act
        await controller.GetElementStatus("token", 1337);

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetElementStatusCommand>(x => x.WebServiceToken == "token"));
    }
}
using AdLerBackend.API.Controllers.Player;
using AdLerBackend.Application.Player.DeletePlayerData;
using AdLerBackend.Application.Player.GetPlayerData;
using AdLerBackend.Application.Player.UpdatePlayerData;
using AdLerBackend.Domain.Entities.PlayerData;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Controllers.Player;

/// <summary>
///     No More Tests are needed, because all Functions are Inherited from GenericRepository
/// </summary>
public class PlayerControllerTest
{
    private IMediator _mediator;

    [SetUp]
    // ANF-ID: [BPG6]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
    }

    [Test]
    // ANF-ID: [BPG6]
    public async Task GetPlayerData_Valid_CallsMediator()
    {
        // Arrange

        var systemUnderTest = new PlayerController(_mediator);

        // Act
        await systemUnderTest.GetPlayer("123");

        // Assert
        await _mediator.Received(1).Send(Arg.Is<GetPlayerDataCommand>(x => x.WebServiceToken == "123"));
    }

    [Test]
    // ANF-ID: [BPG6]
    public async Task UpdatePlayer_CallsMediator()
    {
        // Arrange
        var systemUnderTest = new PlayerController(_mediator);

        // Act
        await systemUnderTest.UpdatePlayer("test", new JsonPatchDocument<PlayerData>());

        // Assert
        await _mediator.Received(1).Send(Arg.Any<UpdatePlayerCommand>());
    }

    [Test]
    public async Task DeleletePlayer_CallsMediator()
    {
        // Arrange
        var systemUnderTest = new PlayerController(_mediator);

        // Act
        await systemUnderTest.DeletePlayer("test");

        // Assert
        await _mediator.Received(1).Send(Arg.Any<DeletePlayerDataCommand>());
    }
}
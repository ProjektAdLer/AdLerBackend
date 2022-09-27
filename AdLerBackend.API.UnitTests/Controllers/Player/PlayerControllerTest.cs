using AdLerBackend.API.Controllers.Player;
using AdLerBackend.Application.Player.GetPlayerData;
using MediatR;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Controllers.Player;

/// <summary>
///     No More Tests are needed, because all Functions are Inherited from GenericRepository
/// </summary>
public class PlayerControllerTest
{
    private IMediator _mediator;

    [Test]
    public async Task GetPlayerData_Valid_CallsMediator()
    {
        // Arrange
        _mediator = Substitute.For<IMediator>();
        var systemUnderTest = new PlayerController(_mediator);

        // Act
        await systemUnderTest.GetPlayer("123");

        // Assert
        await _mediator.Received(1).Send(Arg.Is<GetPlayerDataCommand>(x => x.WebServiceToken == "123"));
    }
}
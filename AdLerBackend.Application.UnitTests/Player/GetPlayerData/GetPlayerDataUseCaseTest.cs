using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Player.GetPlayerData;
using AdLerBackend.Domain.UnitTests.TestingUtils;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.Player.GetPlayerData;

public class GetPlayerDataUseCaseTest
{
    private ILMS _ilms;
    private IPlayerRepository _playerRepository;

    [SetUp]
    public void Setup()
    {
        _ilms = Substitute.For<ILMS>();
        _playerRepository = Substitute.For<IPlayerRepository>();
    }


    [Test]
    public async Task Handle_Valid_GetsPlayerData()
    {
        // Arrange
        var lmsUserDataResponse = new LMSUserDataResponse
        {
            UserEmail = "foo@bar.de",
            UserId = 1,
            LMSUserName = "userName"
        };

        _ilms.GetLMSUserDataAsync(Arg.Any<string>()).Returns(lmsUserDataResponse);


        _playerRepository.GetOrCreatePlayerAsync(1)
            .Returns(PlayerDataEntityFactory.CreatePlayerData());

        var systemUnderTest = new GetPlayerDataUseCase(_ilms, _playerRepository);

        // Act
        var result = await systemUnderTest.Handle(new GetPlayerDataCommand
        {
            WebServiceToken = "testToken"
        }, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(PlayerDataEntityFactory.CreatePlayerData().PlayerGender, Is.EqualTo(result.PlayerGender));
            Assert.That(PlayerDataEntityFactory.CreatePlayerData().PlayerWorldColor,
                Is.EqualTo(result.PlayerWorldColor));
        });
    }
}
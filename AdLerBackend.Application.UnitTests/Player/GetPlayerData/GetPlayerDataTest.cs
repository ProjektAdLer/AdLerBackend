using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Player.GetPlayerData;
using AdLerBackend.Domain.Entities.PlayerData;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.Player.GetPlayerData;

public class GetPlayerDataTest
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
        var systemUnderTest = new GetPlayerDataHandler(_ilms, _playerRepository);

        _ilms.GetLMSUserDataAsync(Arg.Any<string>()).Returns(new LMSUserDataResponse
        {
            IsAdmin = false,
            UserEmail = "foo@bar.de",
            UserId = 1,
            LMSUserName = "userName"
        });

        _playerRepository.EnsureGetAsync(Arg.Any<int>()).Returns(new PlayerData
        {
            Id = 1,
            PlayerGender = PlayerAvatarGender.Female,
            PlayerWorldColor = PlayerWorldColor.Blue
        });

        // Act
        var result = await systemUnderTest.Handle(new GetPlayerDataCommand
        {
            WebServiceToken = "testToken"
        }, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
}
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Player.GetPlayerData;
using AdLerBackend.Domain.Entities.PlayerData;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.Player.GetPlayerData;

public class GetPlayerDataTest
{
    private IMoodle _moodle;
    private IPlayerRepository _playerRepository;

    [SetUp]
    public void Setup()
    {
        _moodle = Substitute.For<IMoodle>();
        _playerRepository = Substitute.For<IPlayerRepository>();
    }

    [Test]
    public async Task Handle_Valid_GetsPlayerData()
    {
        // Arrange
        var systemUnderTest = new GetPlayerDataHandler(_moodle, _playerRepository);

        _moodle.GetMoodleUserDataAsync(Arg.Any<string>()).Returns(new MoodleUserDataResponse
        {
            IsAdmin = false,
            UserEmail = "foo@bar.de",
            UserId = 1,
            MoodleUserName = "userName"
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
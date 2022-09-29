using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Player.DeletePlayerData;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Player.DeletePlayerData;

public class DeletePlayerDataTest
{
    private IMoodle _moodle;
    private IPlayerRepository _playerRepository;

    [SetUp]
    public void Setup()
    {
        _playerRepository = Substitute.For<IPlayerRepository>();
        _moodle = Substitute.For<IMoodle>();
    }

    [Test]
    public async Task Handler_DeletesPlayer()
    {
        // Arrange
        var systemUnderTest = new DeletePlayerDataHandler(_playerRepository, _moodle);

        _moodle.GetMoodleUserDataAsync(Arg.Any<string>()).Returns(new MoodleUserDataResponse
        {
            UserId = 1
        });

        // Act
        var result = await systemUnderTest.Handle(new DeletePlayerDataCommand
        {
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        await _playerRepository.Received(1).DeleteAsync(1);
    }
}
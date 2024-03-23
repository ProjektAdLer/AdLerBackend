using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Player.DeletePlayerData;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Player.DeletePlayerData;

public class DeletePlayerDataUseCaseTest
{
    private ILMS _ilms;
    private IPlayerRepository _playerRepository;

    [SetUp]
    public void Setup()
    {
        _playerRepository = Substitute.For<IPlayerRepository>();
        _ilms = Substitute.For<ILMS>();
    }

    
    [Test]
    public async Task Handler_DeletesPlayer()
    {
        // Arrange
        var systemUnderTest = new DeletePlayerDataUseCase(_playerRepository, _ilms);

        _ilms.GetLMSUserDataAsync(Arg.Any<string>()).Returns(new LMSUserDataResponse
        {
            UserId = 1
        });

        // BPG1

        // Act
        var result = await systemUnderTest.Handle(new DeletePlayerDataCommand
        {
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        await _playerRepository.Received(1).DeleteAsync(1);
    }
}
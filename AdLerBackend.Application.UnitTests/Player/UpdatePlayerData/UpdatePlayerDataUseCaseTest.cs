using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Player.UpdatePlayerData;
using AdLerBackend.Domain.Entities.PlayerData;
using AdLerBackend.Domain.UnitTests.TestingUtils;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.Player.UpdatePlayerData;

public class UpdatePlayerDataUseCaseTest
{
    private ILMS _ilms;
    private IPlayerRepository _playerRepository;

    [SetUp]
    // ANF-ID: [BPG8]
    public void Setup()
    {
        _playerRepository = Substitute.For<IPlayerRepository>();
        _ilms = Substitute.For<ILMS>();
    }

    // ANF-ID: [BPG8]
    [Test]
    public async Task UpdatePlayerData_Valid()
    {
        // Arrange
        var systemUnderTest = new UpdatePlayerDataUseCase(_playerRepository, _ilms);
        _ilms.GetLMSUserDataAsync(Arg.Any<string>()).Returns(new LMSUserDataResponse
        {
            UserId = 1
        });

        _playerRepository.GetOrCreatePlayerAsync(Arg.Any<int>())
            .Returns(PlayerDataEntityFactory.CreatePlayerData());

        var patchDocument = new JsonPatchDocument<PlayerData>();
        patchDocument.Operations.Add(new Operation<PlayerData>("add", "/playergender", "", "0"));

        // Act
        var result = await systemUnderTest.Handle(new UpdatePlayerCommand
        {
            WebServiceToken = "token",
            PatchDocument = patchDocument
        }, CancellationToken.None);

        // Assert
        await _playerRepository.Received(1).GetOrCreatePlayerAsync(Arg.Any<int>());
        await _playerRepository.Received(1).UpdateAsync(Arg.Any<PlayerData>());

        result.PlayerGender.Should().Be(0);
    }
}
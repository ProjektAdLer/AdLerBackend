using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Player;
using MediatR;

namespace AdLerBackend.Application.Player.UpdatePlayerData;

public class UpdatePlayerDataUseCase : IRequestHandler<UpdatePlayerCommand, PlayerDataResponse>
{
    private readonly ILMS _ilms;
    private readonly IPlayerRepository _playerRepository;

    public UpdatePlayerDataUseCase(IPlayerRepository playerRepository, ILMS ilms)
    {
        _playerRepository = playerRepository;
        _ilms = ilms;
    }

    public async Task<PlayerDataResponse> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        // Get Player Id from Moodle
        var playerMoodleData = await _ilms.GetLMSUserDataAsync(request.WebServiceToken);

        // Get Player Data from Database
        var playerData = await _playerRepository.GetOrCreatePlayerAsync(playerMoodleData.UserId);

        // Update Player Data
        request.PatchDocument.ApplyTo(playerData);
        await _playerRepository.UpdateAsync(playerData);

        return new PlayerDataResponse
        {
            PlayerGender = playerData.PlayerGender,
            PlayerWorldColor = playerData.PlayerWorldColor
        };
    }
}
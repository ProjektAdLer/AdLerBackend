using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Player;
using MediatR;

namespace AdLerBackend.Application.Player.UpdatePlayerData;

public class UpdatePlayerDataHandler : IRequestHandler<UpdatePlayerCommand, PlayerDataResponse>
{
    private readonly IMoodle _moodle;
    private readonly IPlayerRepository _playerRepository;

    public UpdatePlayerDataHandler(IPlayerRepository playerRepository, IMoodle moodle)
    {
        _playerRepository = playerRepository;
        _moodle = moodle;
    }

    public async Task<PlayerDataResponse> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        // Get Player Id from Moodle
        var playerMoodleData = await _moodle.GetMoodleUserDataAsync(request.WebServiceToken);

        // Get Player Data from Database
        var playerData = await _playerRepository.EnsureGetAsync(playerMoodleData.UserId);

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
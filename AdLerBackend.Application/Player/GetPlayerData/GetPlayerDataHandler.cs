using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Player;
using AdLerBackend.Domain.Entities.PlayerData;
using MediatR;

namespace AdLerBackend.Application.Player.GetPlayerData;

public class GetPlayerDataHandler : IRequestHandler<GetPlayerDataCommand, PlayerDataResponse>
{
    private readonly IMoodle _moodle;
    private readonly IPlayerRepository _playerDataRepository;

    public GetPlayerDataHandler(IMoodle moodle, IPlayerRepository playerDataRepository1)
    {
        _moodle = moodle;
        _playerDataRepository = playerDataRepository1;
    }

    public async Task<PlayerDataResponse> Handle(GetPlayerDataCommand request, CancellationToken cancellationToken)
    {
        // Get Player Id from Moodle
        var playerMoodleData = await _moodle.GetMoodleUserDataAsync(request.WebServiceToken);

        // Get Player Data from Database
        var playerData = await _playerDataRepository.GetAsync(playerMoodleData.UserId);

        // If Player Data is not in Database, create new Player Data and save it to Database
        if (playerData == null)
        {
            var newPlayerData = new PlayerData
            {
                Id = playerMoodleData.UserId
            };

            await _playerDataRepository.AddAsync(newPlayerData);

            playerData = await _playerDataRepository.GetAsync(playerMoodleData.UserId);
        }


        return new PlayerDataResponse
        {
            Gender = playerData!.PlayerGender,
            WorldColor = playerData.PlayerWorldColor
        };
    }
}
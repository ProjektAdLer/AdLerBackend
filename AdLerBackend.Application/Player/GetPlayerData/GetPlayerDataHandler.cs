using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Player;
using MediatR;

namespace AdLerBackend.Application.Player.GetPlayerData;

public class GetPlayerDataHandler : IRequestHandler<GetPlayerDataCommand, PlayerDataResponse>
{
    private readonly ILMS _ilms;
    private readonly IPlayerRepository _playerDataRepository;

    public GetPlayerDataHandler(ILMS ilms, IPlayerRepository playerDataRepository1)
    {
        _ilms = ilms;
        _playerDataRepository = playerDataRepository1;
    }

    public async Task<PlayerDataResponse> Handle(GetPlayerDataCommand request, CancellationToken cancellationToken)
    {
        // Get Player Id from Moodle
        var playerMoodleData = await _ilms.GetLmsUserDataAsync(request.WebServiceToken);

        // Get Player Data from Database
        var playerData = await _playerDataRepository.EnsureGetAsync(playerMoodleData.UserId);


        return new PlayerDataResponse
        {
            PlayerGender = playerData!.PlayerGender,
            PlayerWorldColor = playerData.PlayerWorldColor
        };
    }
}
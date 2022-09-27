using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Player;
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
        var playerData = await _playerDataRepository.EnsureGetAsync(playerMoodleData.UserId);


        return new PlayerDataResponse
        {
            PlayerGender = playerData!.PlayerGender,
            PlayerWorldColor = playerData.PlayerWorldColor
        };
    }
}
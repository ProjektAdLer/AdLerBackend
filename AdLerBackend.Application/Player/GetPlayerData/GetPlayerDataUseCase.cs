using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.Player;
using MediatR;

namespace AdLerBackend.Application.Player.GetPlayerData;

public class GetPlayerDataUseCase : IRequestHandler<GetPlayerDataCommand, PlayerDataResponse>
{
    private readonly ILMS _ilms;
    private readonly IPlayerRepository _playerDataRepository;

    public GetPlayerDataUseCase(ILMS ilms, IPlayerRepository playerDataRepository1)
    {
        _ilms = ilms;
        _playerDataRepository = playerDataRepository1;
    }

    public async Task<PlayerDataResponse> Handle(GetPlayerDataCommand request, CancellationToken cancellationToken)
    {
        var playerMoodleData = await _ilms.GetLMSUserDataAsync(request.WebServiceToken);

        var playerData = await _playerDataRepository.GetOrCreatePlayerAsync(playerMoodleData.UserId);


        return new PlayerDataResponse
        {
            PlayerGender = playerData!.PlayerGender,
            PlayerWorldColor = playerData.PlayerWorldColor
        };
    }
}
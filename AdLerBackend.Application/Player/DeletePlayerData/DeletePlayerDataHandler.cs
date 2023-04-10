using AdLerBackend.Application.Common.Interfaces;
using MediatR;

namespace AdLerBackend.Application.Player.DeletePlayerData;

public class DeletePlayerDataHandler : IRequestHandler<DeletePlayerDataCommand, bool>
{
    private readonly ILMS _ilms;
    private readonly IPlayerRepository _playerRepository;

    public DeletePlayerDataHandler(IPlayerRepository playerRepository, ILMS ilms)
    {
        _playerRepository = playerRepository;
        _ilms = ilms;
    }

    public async Task<bool> Handle(DeletePlayerDataCommand request, CancellationToken cancellationToken)
    {
        var moodleData = await _ilms.GetLMSUserDataAsync(request.WebServiceToken);
        await _playerRepository.DeleteAsync(moodleData.UserId);

        return true;
    }
}
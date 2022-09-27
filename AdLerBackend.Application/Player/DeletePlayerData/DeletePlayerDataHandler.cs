using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Player.DeletePlayerData;
using MediatR;

namespace AdLerBackend.Application.Player.DeleteUserData;

public class DeletePlayerDataHandler : IRequestHandler<DeletePlayerDataCommand, bool>
{
    private readonly IMoodle _moodle;
    private readonly IPlayerRepository _playerRepository;

    public DeletePlayerDataHandler(IPlayerRepository playerRepository, IMoodle moodle)
    {
        _playerRepository = playerRepository;
        _moodle = moodle;
    }

    public async Task<bool> Handle(DeletePlayerDataCommand request, CancellationToken cancellationToken)
    {
        var moodleData = await _moodle.GetMoodleUserDataAsync(request.WebServiceToken);
        await _playerRepository.DeleteAsync(moodleData.UserId);

        return true;
    }
}
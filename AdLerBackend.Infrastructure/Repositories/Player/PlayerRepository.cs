using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Domain.Entities.PlayerData;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using AdLerBackend.Infrastructure.Repositories.Common;

namespace AdLerBackend.Infrastructure.Repositories.Player;

public class PlayerRepository : GenericRepository<PlayerData>, IPlayerRepository
{
    public PlayerRepository(BaseAdLerBackendDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PlayerData> EnsureGetAsync(int id)
    {
        // Get Player Data from Database
        var playerData = await GetAsync(id);

        if (playerData != null) return playerData!;
        // If Player Data is not in Database, create new Player Data and save it to Database
        var newPlayerData = new PlayerData(PlayerAvatarGender.Male, PlayerWorldColor.Blue, id);

        await AddAsync(newPlayerData);

        playerData = await GetAsync(id);

        return playerData!;
    }
}
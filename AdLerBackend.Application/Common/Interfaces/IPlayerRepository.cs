using AdLerBackend.Domain.Entities.PlayerData;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IPlayerRepository : IGenericRepository<PlayerData, int>
{
    public Task<PlayerData> GetOrCreatePlayerAsync(int id);
}
using AdLerBackend.Domain.Entities.PlayerData;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IPlayerRepository : IGenericRepository<PlayerData>
{
    public Task<PlayerData> GetOrCreatePlayerAsync(int id);
}
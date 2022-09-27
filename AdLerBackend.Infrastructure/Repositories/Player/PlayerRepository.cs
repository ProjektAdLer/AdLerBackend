using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Domain.Entities;
using AdLerBackend.Domain.Entities.PlayerData;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using AdLerBackend.Infrastructure.Repositories.Common;

namespace AdLerBackend.Infrastructure.Repositories.Player;

public class PlayerRepository : GenericRepository<PlayerData>, IPlayerRepository
{
    public PlayerRepository(BaseAdLerBackendDbContext dbContext) : base(dbContext)
    {
    }
}
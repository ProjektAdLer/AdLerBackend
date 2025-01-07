using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Domain.Entities;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using AdLerBackend.Infrastructure.Repositories.Common;

namespace AdLerBackend.Infrastructure.Repositories.Player;

public class PlayerRepository(BaseAdLerBackendDbContext dbContext)
    : GenericRepository<PlayerEntity, int>(dbContext), IPlayerRepository
{
}
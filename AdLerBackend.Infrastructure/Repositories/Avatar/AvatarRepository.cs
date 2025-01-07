using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Domain.Entities;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using AdLerBackend.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace AdLerBackend.Infrastructure.Repositories.Avatar;

public class AvatarRepository(BaseAdLerBackendDbContext dbContext)
    : GenericRepository<AvatarEntity, int>(dbContext), IAvatarRepository
{
    public Task<AvatarEntity?> GetAvatarForPlayerAsync(int requestPlayerId)
    {
        return Context.Avatars.FirstOrDefaultAsync(avatar => avatar.PlayerDataId == requestPlayerId);
    }
}
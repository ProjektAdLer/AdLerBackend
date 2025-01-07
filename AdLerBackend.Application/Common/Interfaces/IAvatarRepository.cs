using AdLerBackend.Domain.Entities;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IAvatarRepository : IGenericRepository<AvatarEntity, int>
{
    Task<AvatarEntity?> GetAvatarForPlayerAsync(int requestPlayerId);
}
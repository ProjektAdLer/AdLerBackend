using AdLerBackend.Domain.Entities;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IPlayerRepository : IGenericRepository<PlayerEntity, int>
{
}
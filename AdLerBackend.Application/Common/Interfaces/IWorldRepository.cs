using AdLerBackend.Domain.Entities;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IWorldRepository : IGenericRepository<WorldEntity, int>
{
    Task<IList<WorldEntity>> GetAllForAuthor(int authorId);
    new Task<WorldEntity?> GetAsync(int id);
}
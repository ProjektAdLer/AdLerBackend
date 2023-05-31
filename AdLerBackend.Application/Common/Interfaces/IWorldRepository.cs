using AdLerBackend.Domain.Entities;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IWorldRepository : IGenericRepository<WorldEntity, int>
{
    Task<IList<WorldEntity>> GetAllForAuthor(int authorId);
    Task<IList<WorldEntity>> GetAllByStrings(List<string> searchStrings);
    new Task<WorldEntity?> GetAsync(int id);
}
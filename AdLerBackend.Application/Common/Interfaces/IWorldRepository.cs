using AdLerBackend.Domain.Entities;

namespace AdLerBackend.Application.Common.Interfaces;

public interface IWorldRepository : IGenericRepository<WorldEntity>
{
    Task<IList<WorldEntity>> GetAllForAuthor(int authorId);

    Task<bool> ExistsForAuthor(int authorId, string courseName);

    Task<IList<WorldEntity>> GetAllByStrings(List<string> searchStrings);
    new Task<WorldEntity?> GetAsync(int id);
}
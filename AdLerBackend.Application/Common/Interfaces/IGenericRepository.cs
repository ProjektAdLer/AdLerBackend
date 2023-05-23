namespace AdLerBackend.Application.Common.Interfaces;

public interface IGenericRepository<T, in TId> where T : class
{
    Task<T?> GetAsync(TId id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task DeleteAsync(TId id);
    Task UpdateAsync(T entity);
    Task<bool> Exists(TId id);
}
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Domain.Entities;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using AdLerBackend.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace AdLerBackend.Infrastructure.Repositories.Worlds;

public class WorldRepository : GenericRepository<WorldEntity>, IWorldRepository
{
    public WorldRepository(BaseAdLerBackendDbContext dbContext) : base(dbContext)
    {
    }


    public async Task<IList<WorldEntity>> GetAllForAuthor(int authorId)
    {
        var allCoursesForAuthor = await Context.Worlds.Where(x => x.AuthorId == authorId).ToListAsync();

        return allCoursesForAuthor;
    }

    public async Task<bool> ExistsForAuthor(int authorId, string courseName)
    {
        var test = await Context.Worlds.AnyAsync(x => x.AuthorId == authorId && x.Name == courseName);
        return test;
    }


    public async Task<IList<WorldEntity>> GetAllByStrings(List<string> searchStrings)
    {
        return await Context.Worlds.Where(c => searchStrings.Contains(c.Name)).ToListAsync();
    }

    public new async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public new async Task<WorldEntity?> GetAsync(int id)
    {
        // Get the course and include the h5pLocations
        var course = await Context.Worlds.Where(c => c.Id == id).Include(c => c.H5PFilesInCourse)
            .FirstOrDefaultAsync();
        return course;
    }
}
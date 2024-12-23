﻿using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Infrastructure.Repositories.BaseContext;
using Microsoft.EntityFrameworkCore;

namespace AdLerBackend.Infrastructure.Repositories.Common;

public class GenericRepository<T, TId>(BaseAdLerBackendDbContext dbContext) : IGenericRepository<T, TId>
    where T : class
{
    protected readonly BaseAdLerBackendDbContext Context = dbContext;

    public async Task<T> AddAsync(T entity)
    {
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(TId id)
    {
        var entity = await GetAsync(id);
        Context.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<bool> Exists(TId id)
    {
        var entity = await GetAsync(id);
        return entity != null;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        Context.Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<T?> GetAsync(TId id)
    {
        return await Context.Set<T>().FindAsync(id);
    }
}
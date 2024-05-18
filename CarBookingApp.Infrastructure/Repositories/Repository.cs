using System.Linq.Expressions;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Model;
using CarBookingApp.Infrastructure.Configurations;
using CarBookingApp.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CarBookingApp.Infrastructure.Repositories;

public class Repository : IRepository
{
    private readonly CarBookingAppDbContext _carBookingAppDbContext;

    public Repository(CarBookingAppDbContext carBookingAppDbContext)
    {
        _carBookingAppDbContext = carBookingAppDbContext;
    }

    public async Task<T> GetByIdAsync<T>(int id) where T : Entity
    {
        var entity = await _carBookingAppDbContext.Set<T>().FindAsync(id);
        if (entity == null)
        {
            throw new EntityNotFoundException($"Object {typeof(T).Name} with id {id} not found.");
        }

        return entity;
    }

    public async Task<T> GetByIdWithInclude<T>(int id,
        params Expression<Func<T, object>>[] includeProperties) where T : Entity
    {
        IQueryable<T> entities = _carBookingAppDbContext.Set<T>();

        foreach (var includeProperty in includeProperties)
        {
            entities = entities.Include(includeProperty);
        }

        var entity = await entities.FirstOrDefaultAsync(entity => entity.Id == id);

        if (entity == null)
        {
            throw new EntityNotFoundException($"Object {typeof(T).Name} with id {id} not found.");
        }

        return entity;
    }

    public async Task<List<T>> GetByPredicate<T>(Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includeProperties) where T : Entity
    {
        IQueryable<T> entities = _carBookingAppDbContext.Set<T>();
        foreach (var includeProperty in includeProperties)
        {
            entities = entities.Include(includeProperty);
        }

        var filteredEntities = await entities.Where(predicate).ToListAsync();
        if (!filteredEntities.Any())
        {
            throw new EntityNotFoundException($"Object {typeof(T).Name} not found.");
        }

        return filteredEntities;
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : Entity
    {
        return await _carBookingAppDbContext.Set<T>().ToListAsync();
    }

    public async Task<T> AddAsync<T>(T entity) where T : Entity
    {
        await _carBookingAppDbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<T> UpdateAsync<T>(T entity) where T : Entity
    {
        _carBookingAppDbContext.Set<T>().Update(entity);
        return entity;
    }

    public async Task<T> DeleteAsync<T>(int id) where T : Entity
    {
        var entityToDelete = await GetByIdAsync<T>(id);
        _carBookingAppDbContext.Set<T>().Remove(entityToDelete);

        return entityToDelete;
    }

    public async Task<T> DeleteAsyncWithInclude<T>(int id,
        params Expression<Func<T, object>>[] includeProperties) where T : Entity
    {
        var entityToDelete = await GetByIdWithInclude(id, includeProperties);
        _carBookingAppDbContext.Set<T>().Remove(entityToDelete);

        return entityToDelete;
    }

    public async Task Save()
    {
        await _carBookingAppDbContext.SaveChangesAsync();
    }
}
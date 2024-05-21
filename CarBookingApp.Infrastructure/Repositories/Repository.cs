using System.Linq.Expressions;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Common.Models;
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
    
    public async Task<PaginatedList<T>> GetAllPaginatedAsync<T>(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        params Expression<Func<T, object>>[] includeProperties) where T : Entity
    {
        IQueryable<T> query = _carBookingAppDbContext.Set<T>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        var totalCount = await query.CountAsync();
        var entities = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<T>(entities, totalCount, pageNumber, pageSize);
    }

    public async Task Save()
    {
        await _carBookingAppDbContext.SaveChangesAsync();
    }
}
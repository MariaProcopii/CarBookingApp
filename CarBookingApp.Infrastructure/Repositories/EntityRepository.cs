using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Model;
using CarBookingApp.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CarBookingApp.Infrastructure.Repositories;

public class EntityRepository : IEntityRepository
{
    private readonly CarBookingAppDbContext _carBookingAppDbContext;

    public EntityRepository(CarBookingAppDbContext carBookingAppDbContext)
    {
        _carBookingAppDbContext = carBookingAppDbContext;
    }

    public async Task<T?> GetByIdAsync<T>(int id) where T : Entity
    {
        return await _carBookingAppDbContext.Set<T>().FindAsync(id);
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

    public async Task<T?> DeleteAsync<T>(int id) where T : Entity
    {
        var entityToDelete = await GetByIdAsync<T>(id);
        if (entityToDelete != null)
        {
            _carBookingAppDbContext.Set<T>().Remove(entityToDelete);
        }

        return entityToDelete;
    }
}
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Model;
using CarBookingApp.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CarBookingApp.Infrastructure.Repositories;

public class EntityRepository<T> : IEntityRepository<T> where T : Entity
{
    private readonly CarBookingAppDbContext _carBookingAppDbContext;

    public EntityRepository(CarBookingAppDbContext carBookingAppDbContext)
    {
        _carBookingAppDbContext = carBookingAppDbContext;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _carBookingAppDbContext.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _carBookingAppDbContext.Set<T>().ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _carBookingAppDbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _carBookingAppDbContext.Set<T>().Update(entity);
        return entity;
    }

    public async Task<T?> DeleteAsync(int id)
    {
        var entityToDelete = await GetByIdAsync(id);
        if (entityToDelete != null)
        {
            _carBookingAppDbContext.Set<T>().Remove(entityToDelete);
        }

        return entityToDelete;
    }
}
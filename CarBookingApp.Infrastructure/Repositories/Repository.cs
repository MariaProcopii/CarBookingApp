using CarBookingApp.Application.Abstractions;
using CarBookingApp.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CarBookingApp.Infrastructure.Repositories;

public class Repository : IRepository
{
    private readonly CarBookingAppDbContext _carBookingAppDbContext;

    public Repository(CarBookingAppDbContext carBookingAppDbContext)
    {
        _carBookingAppDbContext = carBookingAppDbContext;
    }

    public async Task<T?> GetByIdAsync<T>(int id) where T : class
    {
        return await _carBookingAppDbContext.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
    {
        return await _carBookingAppDbContext.Set<T>().ToListAsync();
    }

    public async Task<T> AddAsync<T>(T entity) where T : class
    {
        await _carBookingAppDbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<T> UpdateAsync<T>(T entity) where T : class
    {
        _carBookingAppDbContext.Set<T>().Update(entity);
        return entity;
    }

    public async Task<T?> DeleteAsync<T>(int id) where T : class
    {
        var entityToDelete = await GetByIdAsync<T>(id);
        if (entityToDelete != null)
        {
            _carBookingAppDbContext.Set<T>().Remove(entityToDelete);
        }

        return entityToDelete;
    }
    
    public async Task Save()
    {
        await _carBookingAppDbContext.SaveChangesAsync();
    }
}
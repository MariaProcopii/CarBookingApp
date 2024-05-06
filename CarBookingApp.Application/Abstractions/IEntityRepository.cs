using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Abstractions;
public interface IEntityRepository
{
    Task<T?> GetByIdAsync<T>(int id) where T : Entity;
    Task<IEnumerable<T>> GetAllAsync<T>() where T : Entity;
    Task<T> AddAsync<T>(T entity) where T : Entity;
    Task<T> UpdateAsync<T>(T entity) where T : Entity;
    Task<T?> DeleteAsync<T>(int id) where T : Entity;
}
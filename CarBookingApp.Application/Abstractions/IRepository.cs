using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Abstractions;
public interface IRepository
{
    Task<T?> GetByIdAsync<T>(int id) where T : class;
    Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
    Task<T> AddAsync<T>(T entity) where T : class;
    Task<T> UpdateAsync<T>(T entity) where T : class;
    Task<T?> DeleteAsync<T>(int id) where T : class;
    Task Save();
}
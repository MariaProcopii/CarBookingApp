using System.Linq.Expressions;
using CarBookingApp.Application.Common.Models;
using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Abstractions;
public interface IRepository
{
    Task<T> GetByIdAsync<T>(int id) where T : Entity;
    Task<T> GetByIdWithInclude<T>(int id, 
        params Expression<Func<T, object>>[] includeProperties) where T : Entity;

    public Task<List<T>> GetByPredicate<T>(Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includeProperties) where T : Entity;
    Task<IEnumerable<T>> GetAllAsync<T>() where T : Entity;
    Task<T> AddAsync<T>(T entity) where T : Entity;
    Task<T> UpdateAsync<T>(T entity) where T : Entity;
    Task<T?> DeleteAsync<T>(int id) where T : Entity;

    public Task<T> DeleteAsyncWithInclude<T>(int id, 
        params Expression<Func<T, object>>[] includeProperties) where T : Entity;
    Task Save();

    public Task<PaginatedList<T>> GetAllPaginatedAsync<T>(int pageNumber, int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool ascending = true,
        params Expression<Func<T, object>>[]? includeProperties) where T : Entity;
}
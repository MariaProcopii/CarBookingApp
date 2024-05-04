using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Abstractions;

public interface IUnitOfWork<T> where T : Entity
{
    public IEntityRepository<T> EntityRepository { get; }
    Task Save();
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();   
}
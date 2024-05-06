using CarBookingApp.Domain.Model;

namespace CarBookingApp.Application.Abstractions;

public interface IUnitOfWork
{
    public IEntityRepository EntityRepository { get; }
    Task Save();
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();   
}
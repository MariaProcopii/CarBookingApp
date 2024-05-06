using CarBookingApp.Application.Abstractions;
using CarBookingApp.Domain.Model;
using CarBookingApp.Infrastructure.Configurations;

namespace CarBookingApp.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private CarBookingAppDbContext _carBookingAppDbContext;
    public IEntityRepository EntityRepository { get; private set; }

    public UnitOfWork(CarBookingAppDbContext carBookingAppDbContext, IEntityRepository entityRepository)
    {
        _carBookingAppDbContext = carBookingAppDbContext;
        EntityRepository = entityRepository;
    }


    public async Task Save()
    {
        await _carBookingAppDbContext.SaveChangesAsync();
    }

    public async Task BeginTransaction()
    {
        await _carBookingAppDbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransaction()
    {
        await _carBookingAppDbContext.Database.BeginTransactionAsync();
    }

    public async Task RollbackTransaction()
    {
        await _carBookingAppDbContext.Database.RollbackTransactionAsync();
    }
}
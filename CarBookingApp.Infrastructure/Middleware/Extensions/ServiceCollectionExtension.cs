using CarBookingApp.Application.Abstractions;
using CarBookingApp.Infrastructure.Configurations;
using CarBookingApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarBookingApp.Infrastructure.Middleware.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureServices(this IServiceCollection svcs, IConfiguration conf)
    {
        svcs
            .AddScoped<IEntityRepository, EntityRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddDbContext<CarBookingAppDbContext>(cfg =>
                cfg.UseNpgsql(conf.GetConnectionString("DefaultDB")));
    }
}
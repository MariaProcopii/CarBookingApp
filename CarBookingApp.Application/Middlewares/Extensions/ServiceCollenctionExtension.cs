using System.Reflection;
using CarBookingApp.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CarBookingApp.Application.Middlewares.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplicationServices(this IServiceCollection svcs)
    {
        svcs
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}
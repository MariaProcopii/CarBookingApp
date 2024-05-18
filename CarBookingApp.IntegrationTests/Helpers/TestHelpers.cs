using System.Reflection;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Application.Users.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CarBookingApp.IntegrationTests.Helpers;

public static class TestHelpers
{
    public static IMediator CreateMediator(IRepository repository)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(UpdateUserCommand))));
        services.AddSingleton(repository);
        services.AddAutoMapper(typeof(IRepository));

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetRequiredService<IMediator>();
    }
}
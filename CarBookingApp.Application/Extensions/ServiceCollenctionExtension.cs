using System.Reflection;
using CarBookingApp.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CarBookingApp.Application.Extensions;

public static class ServiceCollectionExtension
{   
    public static void AddApplicationServices(this IServiceCollection svcs)
    {
        svcs
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}
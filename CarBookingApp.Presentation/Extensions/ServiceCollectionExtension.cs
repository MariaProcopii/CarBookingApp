using CarBookingApp.Application.Extensions;
using CarBookingApp.Infrastructure.Extensions;

namespace CarBookingApp.Presentation.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPresentationServices(this IServiceCollection svcs, IConfiguration conf)
    {
        svcs.AddInfrastructureServices(conf);
        svcs.AddApplicationServices();
        svcs.AddControllers();
        svcs.AddEndpointsApiExplorer();
        svcs.AddSwaggerGen();
    }
}
using CarBookingApp.Presentation.Middlewares;

namespace CarBookingApp.Presentation.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app) => app.UseMiddleware<ExceptionHandlingMiddleware>();
}
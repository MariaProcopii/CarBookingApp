using System.Net;
using CarBookingApp.Application.Common.Exceptions;
using CarBookingApp.Infrastructure.Exceptions;
using CarBoolingApp.Presentation.Models;
namespace CarBookingApp.Presentation.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<ExceptionHandlingMiddleware>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("Request: {Method} Path: {Path} Message: {Message}", 
                context.Request.Method, context.Request.Path, ex.Message);
            switch (ex)
            {
                case EntityNotValidException _:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case EntityNotFoundException _:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case ActionNotAllowedException _:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            await CreateExceptionResponseAsync(context, ex);
        }
    }

    private static Task CreateExceptionResponseAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = ex.Message
        }.ToString());
    }
}
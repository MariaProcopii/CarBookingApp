using MediatR;
using Microsoft.Extensions.Logging;

namespace CarBookingApp.Application.Common.Behaviors;


public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,  CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling {typeof(TRequest).Name} at {DateTime.Now}");

        var response = await next();

        _logger.LogInformation($"Handled {typeof(TRequest).Name} successfully at {DateTime.Now}");

        return response;
    }
}
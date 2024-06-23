using CarBookingApp.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Options;
using PayPal.Api;

namespace CarBookingApp.Application.Payments.Command;

public class ExecutePaymentCommand : IRequest<string>
{
    public string PaymentId { get; set; }
    public string PayerId { get; set; }
}


public class ExecutePaymentCommandHandler : IRequestHandler<ExecutePaymentCommand, string>
{
    private readonly IPaymentService _paymentService;

    public ExecutePaymentCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<string> Handle(ExecutePaymentCommand request, CancellationToken cancellationToken)
    {
        var paymentState = await _paymentService.ExecutePaymentAsync(request.PaymentId, request.PayerId);
        return paymentState;
    }
}
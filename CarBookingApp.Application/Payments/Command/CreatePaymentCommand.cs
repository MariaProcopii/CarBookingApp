using CarBookingApp.Application.Abstractions;   
using MediatR;
namespace CarBookingApp.Application.Payments.Command;

public class CreatePaymentCommand : IRequest<string>
{
    public decimal Amount { get; set; }
    public string TipperEmail { get; set; }
    public string DriverEmail { get; set; }
}

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, string>
{
    private readonly IPaymentService _paymentService;

    public CreatePaymentCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<string> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {

        var returnUrl = "http://192.168.0.9:5173/payment/execute";
        var cancelUrl = "http://192.168.0.9:5173/my-rides";
        
        var payment = await _paymentService.CreatePaymentAsync(request.Amount, request.TipperEmail, request.DriverEmail, returnUrl, cancelUrl);
        return payment.GetApprovalUrl();
    }
}

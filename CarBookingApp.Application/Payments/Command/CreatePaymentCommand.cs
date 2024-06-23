using CarBookingApp.Application.Abstractions;   
using MediatR;
namespace CarBookingApp.Application.Payments.Command;

public class CreatePaymentCommand : IRequest<string>
{
    public decimal Amount { get; set; }
    public string TipperEmail { get; set; }
    public string DriverEmail { get; set; }
    public string ReturnUrl { get; set; }
    public string CancelUrl { get; set; }
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
        Console.WriteLine(request.ReturnUrl);
        Console.WriteLine(request.CancelUrl);
        var payment = await _paymentService.CreatePaymentAsync(request.Amount, request.TipperEmail, 
            request.DriverEmail, request.ReturnUrl, request.CancelUrl);
        return payment.GetApprovalUrl();
    }
}

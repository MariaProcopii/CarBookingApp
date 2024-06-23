using PayPal.Api;

namespace CarBookingApp.Application.Abstractions;

public interface IPaymentService
{
    Task<Payment> CreatePaymentAsync(decimal amount, string tipperEmail, string driverEmail, string returnUrl, string cancelUrl);
    Task<string> ExecutePaymentAsync(string paymentId, string payerId);
}
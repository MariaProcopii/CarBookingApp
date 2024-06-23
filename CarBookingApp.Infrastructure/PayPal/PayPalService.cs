using Microsoft.Extensions.Options;
using PayPal.Api;
using CarBookingApp.Application.Abstractions;
using CarBookingApp.Infrastructure.Options;
using Microsoft.Extensions.Logging;

namespace CarBookingApp.Infrastructure.PayPal;

public class PayPalService : IPaymentService
{
    private readonly PayPalSettings _payPalSettings;
    private readonly ILogger<PayPalService> _logger;

    public PayPalService(IOptions<PayPalSettings> payPalSettings, ILogger<PayPalService> logger)
    {
        _logger = logger;
        _payPalSettings = payPalSettings.Value;
    }

    private APIContext GetAPIContext()
    {
        try
        {
            var config = new Dictionary<string, string>
            {
                { "mode", _payPalSettings.Mode },
                { "clientId", _payPalSettings.ClientId },
                { "clientSecret", _payPalSettings.ClientSecret }
            };
            
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            return new APIContext(accessToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing PayPal API context");
            throw;
        }
    }

    public async Task<Payment> CreatePaymentAsync(decimal amount, string tipperEmail, string driverEmail, string returnUrl, string cancelUrl)
    {
        var apiContext = GetAPIContext();

        var payment = new Payment
        {
            intent = "sale",
            payer = new Payer
            {
                payment_method = "paypal",
                payer_info = new PayerInfo
                {
                    email = tipperEmail
                }
            },
            transactions = new List<Transaction>
            {
                new Transaction
                {
                    amount = new Amount
                    {
                        currency = "USD",
                        total = amount.ToString("F2")
                    },
                    payee = new Payee
                    {
                        email = driverEmail
                    },
                    description = $"Tip for Driver ({driverEmail})",
                    note_to_payee = "Such a good driver!"
                }
            },
            redirect_urls = new RedirectUrls
            {
                return_url = returnUrl,
                cancel_url = cancelUrl
            }
        };

        var createdPayment = payment.Create(apiContext);
        // var approvalUrl = createdPayment.links.FirstOrDefault(x => x.rel == "approval_url")?.href;
        return await Task.FromResult(createdPayment);
    }

    public async Task<string> ExecutePaymentAsync(string paymentId, string payerId)
    {
        var apiContext = GetAPIContext();
        
        var paymentExecution = new PaymentExecution { payer_id = payerId };
        var payment = new Payment { id = paymentId };

        var executedPayment = payment.Execute(apiContext, paymentExecution);

        return await Task.FromResult(executedPayment.state);
    }
}

using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services.Dto;
using Stripe;
using Stripe.Checkout;

namespace MeApuntoBackend.Services;
public class StripeService : PaymentBaseService
{
    private string? privateKey;
    public override void SetConfiguration(IConfiguration configuration)
    {
        string? strypeKey = configuration?.GetValue<string>("StripeKey");
        privateKey = strypeKey ?? string.Empty;
        StripeConfiguration.ApiKey = privateKey;
    }

    public override PaymentResponseDto ProccessPayment(PaymentDto payment)
    {
        var domain = "http://localhost:5173/payment";
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    Price = payment.ProductId,
                    Quantity = 1,
                  },
                },
            Mode = "payment",
            SuccessUrl = domain + "?success=true",
            CancelUrl = domain + "?success=false",
        };
        var service = new SessionService();
        Session session = service.Create(options);
        return new PaymentResponseDto() { Url = session.Url, Success = session.Status == "open" };
    }
}

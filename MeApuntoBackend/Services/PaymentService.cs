using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services.Dto;
using MeApuntoBackend.Services.Factory;
using MeApuntoBackend.Services.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace MeApuntoBackend.Services;
public class PaymentService : IPaymentService
{
    private readonly IConfiguration _configuration;
    public PaymentService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public PaymentResponseDto ProccessPayment(PaymentDto payment)
    {
        PaymentBaseService stripeService = PaymentServiceFactory.Resolve(payment);
        stripeService.SetConfiguration(_configuration);
        var response = stripeService.ProccessPayment(payment);
        return response;
    }
}

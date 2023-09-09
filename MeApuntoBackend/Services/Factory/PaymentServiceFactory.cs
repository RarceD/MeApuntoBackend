using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services.Factory;
public class PaymentServiceFactory
{
    public static PaymentBaseService Resolve(PaymentDto payment)
    {
        // TODO: When neccessary return other payment service
        return new StripeService();
    }
}

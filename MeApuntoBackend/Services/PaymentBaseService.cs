using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services.Dto;

namespace MeApuntoBackend.Services;
public abstract class PaymentBaseService
{
    public abstract void SetConfiguration(IConfiguration configuration);
    public abstract PaymentResponseDto ProccessPayment(PaymentDto payment);
}

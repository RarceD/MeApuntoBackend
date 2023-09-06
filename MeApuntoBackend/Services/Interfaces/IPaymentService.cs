using MeApuntoBackend.Services.Dto;

namespace MeApuntoBackend.Services.Interfaces;
public interface IPaymentService
{
    PaymentResponseDto ProccessPayment();
}
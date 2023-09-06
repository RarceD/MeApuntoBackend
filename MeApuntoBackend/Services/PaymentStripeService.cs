using MeApuntoBackend.Services.Dto;
using MeApuntoBackend.Services.Interfaces;
using Stripe;

namespace MeApuntoBackend.Services;
public class PaymentStripeService : IPaymentService
{
    private Charge _charge;
    public PaymentStripeService()
    {
    }

    private string CreateToken()
    {
        try
        {
            StripeConfiguration.ApiKey = string.Empty;
            var tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = string.Empty,
                    ExpYear = string.Empty,
                    ExpMonth = string.Empty,
                    Cvc = string.Empty,
                    Name = string.Empty,
                    AddressLine1 = string.Empty,
                    AddressLine2 = "",
                    AddressCity = string.Empty,
                    AddressZip = string.Empty,
                    AddressState = string.Empty,
                    AddressCountry = "MX",
                    Currency = "mxn"
                }
            };

            TokenService tokenService = new();
            Token stripeToken = tokenService.Create(tokenOptions);
            return stripeToken.Id;
        }
        catch (StripeException ex)
        {
            Console.Write("Error creating token" + ex.Message);
            throw ex;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private bool MakePayment(string token)
    {
        try
        {
            StripeConfiguration.ApiKey = string.Empty;// _stripeSecrets.SecretKey;
            var options = new ChargeCreateOptions
            {
                Amount = 1,// _dtoCreditDebitCard.MontoPagado,
                Currency = "mxn",
                Description = "", //  _dtoCreditDebitCard.Descripcion,
                Source = token,
                StatementDescriptor = "", //= _dtoCreditDebitCard.DescripcionEstadoCuenta,
                Capture = true,
                ReceiptEmail = ""//_dtoCreditDebitCard.Correo,
            };
            //Make Payment
            var service = new ChargeService();
            _charge = service.Create(options);
            if (_charge.Status.ToLower().Equals("succeeded"))
            {
                return true;
            }
            return false;
        }
        catch (StripeException ex)
        {
            Console.Write("Payment Gateway" + ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            Console.Write("Payment Gatway (CreateCharge)" + ex.Message);
            throw;
        }
    }

    public PaymentResponseDto ProccessPayment()
    {
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        CancellationToken token = tokenSource.Token;
        bool isTransectionSuccess = false;
        try
        {
            var tokenGenerated = CreateToken();
            Console.Write("Payment Gateway" + "Token :" + tokenGenerated);
            isTransectionSuccess = tokenGenerated != null && MakePayment(tokenGenerated);
            // return _charge;
            return new PaymentResponseDto() { Success = isTransectionSuccess };
        }
        catch (StripeException ex)
        {
            Console.Write("Payment Gateway" + ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            Console.Write("Payment Gateway" + ex.Message);
            throw;
        }
        finally
        {
            if (isTransectionSuccess)
            {
                Console.Write("Payment Gateway" + "Payment Successful ");
            }
            else
            {
                Console.Write("Payment Gateway" + "Payment Failure ");
            }
        }
    }
}

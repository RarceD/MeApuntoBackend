using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services;
public interface IMailService
{
    bool SendCanceledEmail(string toMailAddress, string content);
    bool SendConfirmationEmail(string toMailAddress, string content);
    bool SendEmail(string toMailAddress, string title, string content);
    bool SendResetPasswordEmail(string toMailAddress, string content);
}

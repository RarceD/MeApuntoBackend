using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services;
public interface IMailService
{
    bool SendCanceledEmail(string toMailAddress, string day, string hour, string time);
    bool SendConfirmationEmail(string toMailAddress, string day, string hour, string time);
    bool SendResetPasswordEmail(string toMailAddress, string newPass);
}

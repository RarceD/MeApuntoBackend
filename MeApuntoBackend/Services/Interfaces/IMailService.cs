using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services;
public interface IMailService
{
    bool SendEmail(string username, string content);
}

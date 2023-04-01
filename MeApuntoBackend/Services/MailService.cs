using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;

namespace MeApuntoBackend.Services;
public class MailService : IMailService
{
    public MailService()
    {
    }
    public bool SendEmail(string username, string content)
    {
        return false;
    }
}

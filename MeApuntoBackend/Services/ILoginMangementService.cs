using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services;
public interface ILoginManagementService
{
    LoginResponse CheckUserExist(string user, string pass);
}

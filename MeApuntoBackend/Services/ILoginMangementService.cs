using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services;
public interface IClientManagementService
{
    LoginResponse CheckUserExist(string user, string pass);
    int GetValidUrbaKeyId(string key);
    bool IsValidUserCode(string code);
    bool AddClient(CreateDto newClient, int urbaId);

}

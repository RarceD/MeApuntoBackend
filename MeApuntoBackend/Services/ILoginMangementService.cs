namespace MeApuntoBackend.Services;
public interface ILoginManagementService
{
    bool CheckUserExist(string user, string pass);
}

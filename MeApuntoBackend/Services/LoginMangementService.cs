using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;

namespace MeApuntoBackend.Services;
public class LoginManagementService : ILoginManagementService
{
    private readonly IClientRepository _clientRepository;
    public LoginManagementService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public bool CheckUserExist(string user, string pass)
    {
        List<ClientDb> clients = _clientRepository.GetAll().ToList();
        return true;
    }
}

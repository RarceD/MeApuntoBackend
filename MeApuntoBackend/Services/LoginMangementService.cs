using MeApuntoBackend.Controllers.Dtos;
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
    public LoginResponse CheckUserExist(string user, string pass)
    {
        var resp = new LoginResponse() {Success = false } ;
        ClientDb? client = _clientRepository.GetClientWithUser(user);
        if (client == null) return resp;
        if (pass == client.pass)
        {
            resp.Success = true;
            resp.Token = client.token;
            resp.Id = client.id;
        }
        return resp;
    }
}

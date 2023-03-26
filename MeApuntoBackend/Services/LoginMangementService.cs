using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;

namespace MeApuntoBackend.Services;
public class ClientManagementService : IClientManagementService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUrbaRepository _urbaRepository;
    public ClientManagementService(IClientRepository clientRepository, IUrbaRepository urbaRepository)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
    }

    public bool AddClient(CreateDto newClient, int urbaId)
    {
        string tokenRnd = "";
        ClientDb client = new ClientDb() { 
             urba_id = urbaId,
             name = newClient.Name,
             username = newClient.User,
             pass = newClient.Pass,
             token = tokenRnd,
             floor = newClient.Floor,
             letter = newClient.Door,
             house = newClient.House,
        };
        try
        {
            _clientRepository.Add(client);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
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

    public int GetValidUrbaKeyId(string key)
    {
        var urba = _urbaRepository.GetUrbatWithToken(key);
        if (urba == null) return 0;
        return urba.Id;
    }

    public bool IsValidUserCode(string code)
    {
        return _clientRepository.IsValidUserCode(code);
    }
}

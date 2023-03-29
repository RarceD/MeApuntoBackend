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
        try
        {
            ClientDb client = new ClientDb()
            {
                urba_id = urbaId,
                name = newClient.Name,
                username = newClient.User,
                pass = newClient.Pass,
                token = Utils.Sha256(DateTime.Now.ToString()),
                floor = newClient.Floor,
                letter = newClient.Door,
                house = newClient.House,
                plays = 0
            };
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
        var resp = new LoginResponse() { Success = false };
        ClientDb? client = _clientRepository.GetClientWithUser(user);
        if (client == null) return resp;
        if (pass == client.pass)
        {
            resp.Success = true;
            resp.Token = client.token;
            resp.Id = (int)client.id;
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
    public ProfileResponse GetProfileInfo(int id)
    {
        ProfileResponse profile = new();

        // Obtein the profile:
        ClientDb? client = _clientRepository.GetById(id);
        if (client == null) return profile;

        // Obtein urba name:
        UrbaDb urba = _urbaRepository.GetById(client.urba_id);
        if (client == null) return profile;

        return convertToDto(client, urba.name ?? "");
    }
    private ProfileResponse convertToDto(ClientDb profile, string urbaName)
    {
        ProfileResponse profileResponse = new ProfileResponse();    
        profileResponse.letter = profile.letter;
        profileResponse.username = profile.username;
        profileResponse.urbaName = urbaName; 
        profileResponse.Name = profile.name;
        profileResponse.plays = (int)profile.plays; 
        return profileResponse;
    }

}

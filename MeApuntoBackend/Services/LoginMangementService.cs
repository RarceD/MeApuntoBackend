using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;

namespace MeApuntoBackend.Services;
public class ClientManagementService : IClientManagementService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUrbaRepository _urbaRepository;
    private readonly IMailService _mailService;
    private readonly ILogger<ClientManagementService> _logger;

    private const string FORGET_PASS_TITLE = "[MEAPUNTO.ONLINE] Recuperación de contraseña";

    public ClientManagementService(
        ILogger<ClientManagementService> logger,
        IClientRepository clientRepository,
        IUrbaRepository urbaRepository,
        IMailService mailService
        )
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
        _mailService = mailService;
        _logger = logger;
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
                token = Utils.GetSha256(DateTime.Now.ToString()),
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
        profileResponse.plays = profile.plays;
        return profileResponse;
    }

    public bool UpdateUserProfile(ProfileDto profile)
    {
        // Get original profile:
        ClientDb? client = _clientRepository.GetById(profile.Id);
        if (client == null) return false;

        // Update profile only if changed:
        if (profile.Username != string.Empty)
        {
            // Remove spaces in mail change
            client.username = profile.Username?.Replace(" ", "");
        }
        if (profile.Password != string.Empty) client.pass = profile.Password;

        // Update profile:
        try
        {
            _clientRepository.Update(client);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool CheckUserTokenId(string token, int id)
    {
        ClientDb? client = _clientRepository.GetById(id);
        if (client == null) return false;
        var notCheting = client.token == token;
        if (!notCheting)
            _logger.LogWarning("Client with id: " + id + " has modified token and id");
        return client.token == token;
    }
    public bool ForgetPassword(string username)
    {
        // Get client id:
        ClientDb? client = _clientRepository.GetClientWithUser(username);
        if (client == null) return false;

        // Generate new pass:
        var rawPass = new Random().Next(1000, 5000).ToString();
        client.pass = Utils.GetMD5(rawPass).ToLower();

        // Update db:
        _clientRepository.Update(client);

        if (client.username != null)
        {
            string mailContent = $"Se ha restaurado su contraseña, para acceder su correo es: {client.username} y su constraseña {rawPass}";
            _mailService.SendEmail(client.username, FORGET_PASS_TITLE, mailContent);
            return true;
        }
        return false;
    }

    public string GenerateUrlForCode(string code)
    {
        var extracted = code.Split('.');
        var urbaKey = GetUrbaKey(extracted[0]);
        var house = extracted[1];
        var floor = extracted[2];
        var door = extracted[3];

        // The code must be returned in base 64:
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(code);
        code = Convert.ToBase64String(plainTextBytes);

        // TODO: Return free url in case of free urba:
        // return Config.PUBLIC_URL_FREE + "/create?k=" + urbaKey + "&d=" + door + "&h=" + house + "&f=" + floor + "&i=" + code;

        return Config.PUBLIC_URL + "/create?k=" + urbaKey + "&d=" + door + "&h=" + house + "&f=" + floor + "&i=" + code;
    }


    private string GetUrbaKey(string urbaCode)
    {
        if (Config.URBA_CODES.TryGetValue(urbaCode, out int existCode))
        {
            var urba = _urbaRepository.GetById(existCode);
            if (urba != null)
            {
                return urba.key ?? string.Empty;
            }
        }
        throw new Exception("Urba code does not exist");
    }
}

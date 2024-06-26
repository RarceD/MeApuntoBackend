using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services.Dto;
using System.Text.RegularExpressions;

namespace MeApuntoBackend.Services;
public class ClientManagementService : IClientManagementService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUrbaRepository _urbaRepository;
    private readonly IUrbaCodesRepository _urbaCodesRepository;
    private readonly IMailService _mailService;
    private readonly ILogger<ClientManagementService> _logger;
    private readonly ILoginStatsRepository _loginStatsRepository;
    private readonly IStatsService _statsService;

    public ClientManagementService(
        ILogger<ClientManagementService> logger,
        IClientRepository clientRepository,
        IUrbaRepository urbaRepository,
        IMailService mailService,
        IStatsService statsService,
        ILoginStatsRepository loginStatsRepository,
        IUrbaCodesRepository urbaCodesRepository)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
        _mailService = mailService;
        _logger = logger;
        _statsService = statsService;
        _loginStatsRepository = loginStatsRepository;
        _urbaCodesRepository = urbaCodesRepository;
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
                plays = 0,
                role = 0
            };
            _clientRepository.Add(client);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogWarning("[CREATE] Not able add ClientDb" + e.Message);
            return false;
        }
    }

    public LoginResponse CheckUserExist(string user, string pass)
    {
        // user to lowercase:
        user = user.ToLower();
        var resp = new LoginResponse() { Success = false };
        ClientDb? client = _clientRepository.GetClientWithUser(user);
        if (client != null && pass == client.pass)
        {
            resp.Success = true;
            resp.Token = client.token;
            resp.Id = (int)client.id;
        }
        _statsService.AddLoginRecord(new LoginRecord() { Success = resp.Success, RegisterTime = DateTime.Now });
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
        // Check the format is like: XXX.0.0.0
        if (!Regex.IsMatch(code, @"^[a-zA-Z0-9.]+$"))
        {
            _logger.LogWarning("[CREATE] invalid user code regex validator");
            return false;
        }

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
        if (token == string.Empty || token is null) return false;
        var notCheting = client.token == token;
        if (!notCheting)
            _logger.LogWarning("Client with id: " + id + " has modified token and id");
        return client.token == token;
    }

    public bool CheckUserIsAdmin(int id)
    {
        ClientDb client = _clientRepository.GetById(id);
        return client.role == (int)ClientRole.ADMIN;
    }
    public bool ForgetPassword(string username)
    {
        username = username.ToLower().Replace(" ", "");
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
            _mailService.SendResetPasswordEmail(client.username, rawPass);
            return true;
        }
        return false;
    }

    public string GenerateUrlForCode(string code)
    {
        var extracted = code.Split('.');
        var urba = GetUrbaKey(extracted[0]);
        var house = extracted[1];
        var floor = extracted[2];
        var door = extracted[3];

        // The code must be returned in base 64:
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(code);
        code = Convert.ToBase64String(plainTextBytes);

        var baseUrl = urba.free ? Config.PUBLIC_URL_FREE : Config.PUBLIC_URL_PAID;
        var url = $"{baseUrl}/create?k={urba.urbaKey}&d={door}&h={house}&f={floor}&i={code}";
        return url;
    }


    private (string urbaKey, bool free) GetUrbaKey(string urbaCode)
    {
        Dictionary<string, int> urbaCodesDict = _urbaCodesRepository
            .GetAll()
            .ToDictionary(
                code => code.Code ?? string.Empty,
                code => code.UrbaId
        );

        if (urbaCodesDict.TryGetValue(urbaCode, out int existCode))
        {
            var urba = _urbaRepository.GetById(existCode);
            if (urba != null)
            {
                return (urba.key ?? string.Empty, urba.free);
            }
        }
        throw new Exception("Urba code does not exist");
    }
    public IEnumerable<AdminDto> GetEmailContains(string str)
    {
        if (string.IsNullOrEmpty(str)) return Enumerable.Empty<AdminDto>();
        return _clientRepository.GetAll()
            .Where(client => client.username.Contains(str))
            .Select((client) => new AdminDto()
            {
                Code = client.name,
                Email = client.username
            });
    }
    public IEnumerable<AdminDto> GetCodeContains(string str)
    {
        if (string.IsNullOrEmpty(str)) return Enumerable.Empty<AdminDto>();
        return _clientRepository.GetAll()
                .Where(client => client.name != null && client.name.ToLower().Contains(str))
                .Select((client) => new AdminDto()
                {
                    Code = client.name,
                    Email = client.username
                });
    }
    public IEnumerable<StatsResponse> GetStats()
    {
        return _loginStatsRepository.GetAll()
            .Select((stats) => new StatsResponse()
            {
                Date = stats.RegisterTime,
                Success = stats.Success
            });
    }

    public bool RemoveClient(string username)
    {
        ClientDb? clientToDelete = _clientRepository.GetClientWithUser(username);
        if (clientToDelete is null) return false;
        _clientRepository.Remove(clientToDelete);
        return true;
    }
}

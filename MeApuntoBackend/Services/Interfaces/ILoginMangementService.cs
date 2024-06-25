using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services;
public interface IClientManagementService
{
    LoginResponse CheckUserExist(string user, string pass);
    int GetValidUrbaKeyId(string key);
    bool IsValidUserCode(string code);
    bool AddClient(CreateDto newClient, int urbaId);
    bool RemoveClient(string username);
    ProfileResponse? GetProfileInfo(int id);
    bool UpdateUserProfile(ProfileDto profileId);
    bool CheckUserTokenId(string token, int id);
    bool CheckUserIsAdmin(int id);
    bool ForgetPassword(string username);
    string GenerateUrlForCode(string code);
    IEnumerable<AdminDto> GetEmailContains(string str);
    IEnumerable<AdminDto> GetCodeContains(string str);
    IEnumerable<StatsResponse> GetStats();
}

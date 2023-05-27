using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : GenericController
{
    private readonly ILogger<LoginController> _logger;
    public ProfileController(ILogger<LoginController> logger, IClientManagementService loginManagementService)
        : base(loginManagementService)
    {
        _logger = logger;
    }

    [HttpGet]
    public ProfileResponse GetProfileInfo(string token, int id)
    {
        if (!CheckUserTokenId(token, id))  return new ProfileResponse(); 

        ProfileResponse? profile = _clientManagementService.GetProfileInfo(id);
        return profile == null ? new ProfileResponse() : profile;
    }

    [HttpPost]
    public ActionResult UpdateProfileInfo(ProfileDto input)
    {
        if (!CheckUserTokenId(input.Token?? string.Empty, input.Id)) return NoContent();
        var success = _clientManagementService.UpdateUserProfile(input);
        if (success)
        {
            if (input.Username != string.Empty) 
            {
                _logger.LogWarning("Client id: " + input.Id + " has change its username");
            }
            else
            {
                _logger.LogWarning("Client id: " + input.Id + " has change its password");
            }
        }
        else
        {
            _logger.LogError("Client id: " + input.Id + " not able to change its user/pass");
        }
        return success ? Ok() : NoContent();
    }


}
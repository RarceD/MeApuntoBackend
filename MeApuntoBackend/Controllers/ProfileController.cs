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
        if (profile == null) { return new ProfileResponse(); }
        return profile;
    }

    [HttpPost]
    public ActionResult UpdateProfileInfo(CreateDto input)
    {
        // if (!CheckUserTokenId(token, id))  return NoContent(); 
        var success = _clientManagementService.UpdateUserProfile(input);
        return success ? Ok() : NoContent();
    }


}
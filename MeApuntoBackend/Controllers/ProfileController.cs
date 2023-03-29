using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly IClientManagementService _loginManagementService;
    public ProfileController(ILogger<LoginController> logger, IClientManagementService loginManagementService)
    {
        _logger = logger;
        _loginManagementService = loginManagementService;
    }

    [HttpGet]
    public ProfileResponse GetProfileInfo(ProfileDto input)
    {
        ProfileResponse? profile = _loginManagementService.GetProfileInfo(input.Id);
        if (profile == null) { return new ProfileResponse(); }
        return profile;
    }

    [HttpPost]
    public ActionResult UpdateProfileInfo(CreateDto input)
    {
        var resp = NoContent();
        return resp;
    }


}
using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : GenericController
{
    private readonly ILogger<LoginController> _logger;
    public BookController(ILogger<LoginController> logger, IClientManagementService loginManagementService)
        : base(loginManagementService)
    {
        _logger = logger;
    }

    [HttpGet]
    public ProfileResponse GetBooks(string token, int id)
    {
        if (!CheckUserTokenId(token, id)) return new ProfileResponse();
        ProfileResponse? profile = _clientManagementService.GetProfileInfo(id);
        if (profile == null) { return new ProfileResponse(); }
        return profile;
    }

    [HttpPost]
    public ActionResult AddBookToDb(CreateDto input)
    {
        var success = _clientManagementService.UpdateUserProfile(input);
        return success ? Ok() : NoContent();
    }

    [HttpPost("delete")]
    public ActionResult Delte(CreateDto input)
    {
        // if (!CheckUserTokenId(token, id))  return NoContent(); 
        return true ? Ok() : NoContent();
    }
}
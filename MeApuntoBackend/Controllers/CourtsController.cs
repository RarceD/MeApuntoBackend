using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourtsController : GenericController
{
    private readonly ILogger<LoginController> _logger;
    private readonly ICourtManagementService _courtManagementService;
    public CourtsController(ILogger<LoginController> logger,
        IClientManagementService loginManagementService,
        ICourtManagementService courtManagementService)
        : base(loginManagementService)
    {
        _logger = logger;
        _courtManagementService = courtManagementService;
    }

    [HttpGet]
    public IEnumerable<CourtResponse> GetBooks(string token, int id)
    {
        var c = new List<CourtResponse>();
        if (!CheckUserTokenId(token, id)) return c;

        var profile = _courtManagementService.GetCourts(id);
        if (profile == null) return c; 
        return profile;
    }
}
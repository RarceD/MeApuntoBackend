using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NormativeController : GenericController
{
    private readonly ILogger<LoginController> _logger;
    private readonly ICourtManagementService _courtManagementService;
    public NormativeController(ILogger<LoginController> logger,
        IClientManagementService loginManagementService,
        ICourtManagementService courtManagementService)
        : base(loginManagementService)
    {
        _logger = logger;
        _courtManagementService = courtManagementService;
    }

    [HttpGet]
    public IEnumerable<NormativeResponse> GetNormative(string token, int id)
    {
        var normatives = new List<NormativeResponse>();

        if (!CheckUserTokenId(token, id)) return normatives;
        IEnumerable<NormativeResponse> n =  _courtManagementService.GetNormativeByClientId(id);
        if (n == null) return normatives;
        return n;
    }
}
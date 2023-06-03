using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NormativeController : GenericController
{
    private readonly ICourtManagementService _courtManagementService;
    public NormativeController(IClientManagementService loginManagementService,
        ICourtManagementService courtManagementService)
        : base(loginManagementService)
    {
        _courtManagementService = courtManagementService;
    }

    [HttpGet]
    [ResponseCache(Duration = 60 * 10)] // 10min cache
    public IEnumerable<NormativeResponse> GetNormative(string token, int id)
    {
        var normatives = new List<NormativeResponse>();

        if (!CheckUserTokenId(token, id)) return normatives;
        IEnumerable<NormativeResponse> n = _courtManagementService.GetNormativeByClientId(id);
        if (n == null) return normatives;
        return n;
    }
}
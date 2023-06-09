using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;
public class GenericController : ControllerBase
{
    protected readonly IClientManagementService _clientManagementService;
    public GenericController(IClientManagementService clientManagementService)
    {
        _clientManagementService = clientManagementService;
    }
    protected bool CheckUserTokenId(string token, int id) =>
        _clientManagementService.CheckUserTokenId(token, id);
    protected ActionResult Success() =>
        Ok(new GenericResponseDto() { Error = false });
    protected ActionResult Error() =>
        Ok(new GenericResponseDto() { Error = true });

}
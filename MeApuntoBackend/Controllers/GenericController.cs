using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;
public class GenericController : ControllerBase
{
    protected readonly IClientManagementService _clientManagementService;
    private const string NotAdminResponseMsg = "You are not an Admin, fuck you";
    public GenericController(IClientManagementService clientManagementService)
    {
        _clientManagementService = clientManagementService;
    }
    protected bool CheckUserTokenId(string token, int id) =>
        _clientManagementService.CheckUserTokenId(token, id);
    private bool CheckUserIsAdmin(int id) =>
        _clientManagementService.CheckUserIsAdmin(id);
    protected bool IsAdmin(int id, string token) =>
         CheckUserTokenId(token, id) && CheckUserIsAdmin(id);
    protected IActionResult GetNotAdminResponse() =>
        NotFound(NotAdminResponseMsg);
    protected ActionResult Success() =>
        Ok(new GenericResponseDto() { Error = false });
    protected ActionResult Error() =>
        Ok(new GenericResponseDto() { Error = true });

}
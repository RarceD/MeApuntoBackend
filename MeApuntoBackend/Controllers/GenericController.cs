using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;
public class GenericController : ControllerBase
{
    bool CheckUserToken(string token, int id)
    {
        return true;
    }
}
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginManagementService _loginManagementService;
        public LoginController(ILogger<LoginController> logger, ILoginManagementService loginManagementService)
        {
            _logger = logger;
            _loginManagementService = loginManagementService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<int> Get()
        {
            string user = "", pass = "";
            bool ok = _loginManagementService.CheckUserExist(user, pass);
            return new List<int> { ok ? 1 : 0, 2, 3 };
        }
    }
}
using MeApuntoBackend.Controllers;
using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UniTest.Controllers;

[TestFixture]
public class LoginControllerTests
{
    private ILogger<LoginController> _logger;
    private IClientManagementService _loginManagementService;
    private LoginController _loginController;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<LoginController>>();
        _loginManagementService = Substitute.For<IClientManagementService>();
        _loginController = new LoginController(_logger, _loginManagementService);
    }

    [Test]
    public void MakeLogin_WhenCalled_ReturnsLoginResponse()
    {
        // Arrange
        var input = new LoginDto { User = "User", Pass = "Pass" };
        var response = new LoginResponse { Success = true, Id = 1, Token = "Token" };
        _loginManagementService.CheckUserExist(input.User, input.Pass).Returns(response);

        // Act
        var result = _loginController.MakeLogin(input);

        // Assert
        //Assert.AreEqual(response.Success, result.Success);
        //Assert.AreEqual(response.Id, result.Id);
        //Assert.AreEqual(response.Token, result.Token);
    }
}

using MeApuntoBackend.Controllers;
using MeApuntoBackend.Services;
using NSubstitute;

namespace UniTest.Controllers;

[TestFixture]
public class AdminControllerTests
{
    private IClientManagementService _clientManagementService;
    private AdminController _adminController;

    [SetUp]
    public void Setup()
    {
        _clientManagementService = Substitute.For<IClientManagementService>();
        _adminController = new AdminController(_clientManagementService);
    }

    [Test]
    public void GetMatchCode_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var token = "Token";
        var id = 1;
        var matchStr = "MatchStr";
        // _clientManagementService.IsAdmin(id, token).Returns(true);
        // _clientManagementService.GetCodeContains(matchStr.ToLower()).Returns(new List<string>());

        // Act
        var result = _adminController.GetMatchCode(token, id, matchStr);

        // Assert
        // Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public void GetMatchEmail_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var token = "Token";
        var id = 1;
        var matchStr = "MatchStr";
        // _clientManagementService.IsAdmin(id, token).Returns(true);
        // _clientManagementService.GetEmailContains(matchStr.ToLower()).Returns(new List<string>());

        // Act
        // var result = _adminController.GetMatchEmail(token, id, matchStr);

        // Assert
        // Assert.IsInstanceOf<OkObjectResult>(result);
    }
}

using MeApuntoBackend.Controllers;
using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UniTest.Controllers;

[TestFixture]
public class CourtsControllerTests
{
    private IClientManagementService _loginManagementService;
    private ICourtManagementService _courtManagementService;
    private CourtsController _courtsController;

    [SetUp]
    public void Setup()
    {
        _loginManagementService = Substitute.For<IClientManagementService>();
        _courtManagementService = Substitute.For<ICourtManagementService>();
        _courtManagementService = Substitute.For<ICourtManagementService>();
        var logger = Substitute.For<ILogger<LoginController>>();
        _courtsController = new CourtsController(logger, _loginManagementService, _courtManagementService);
    }

    [Test]
    public void GetBooks_WhenCalled_ReturnsListOfCourtResponse()
    {
        // Arrange
        var token = "Token";
        var id = 1;
        var response = new List<CourtResponse>();
        _loginManagementService.CheckUserTokenId(token, id).Returns(true);
        _courtManagementService.GetCourts(id).Returns(response);

        // Act
        var result = _courtsController.GetBooks(token, id);

        // Assert
        // Assert.AreEqual(response, result);
    }
}

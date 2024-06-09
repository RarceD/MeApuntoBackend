using MeApuntoBackend.Controllers;
using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
namespace UniTest.Controllers;

[TestFixture]
public class BookControllerTests
{
    private ILogger<LoginController> _logger;
    private IBookerManagementService _bookerManagementService;
    private IStatsService _statsService;
    private IClientManagementService _loginManagementService;
    private BookController _bookController;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<LoginController>>();
        _bookerManagementService = Substitute.For<IBookerManagementService>();
        _statsService = Substitute.For<IStatsService>();
        _loginManagementService = Substitute.For<IClientManagementService>();
        _bookController = new BookController(_logger, _loginManagementService, _statsService, _bookerManagementService);
    }

    [Test]
    public void GetBooks_WhenCalled_ReturnsListOfBookerResponse()
    {
        // Arrange
        var token = "Token";
        var id = 1;
        var response = new List<BookerResponse>();
        _bookerManagementService.GetBooks(id).Returns(response);

        // Act
        var result = _bookController.GetBooks(token, id);

        // Assert
        // Assert.AreEqual(response, result);
    }

    // Add more tests for the other methods in the BookController
}

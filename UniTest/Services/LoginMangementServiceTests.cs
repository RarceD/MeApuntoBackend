using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UniTest.Services;

[TestFixture]
public class LoginMangementServiceTests
{
    private IClientRepository _clientRepository;
    private IUrbaRepository _urbaRepository;
    private IUrbaCodesRepository _urbaCodesRepository;
    private IMailService _mailService;
    private ILogger<ClientManagementService> _logger;
    private ILoginStatsRepository _loginStatsRepository;
    private IStatsService _statsService;
    private ClientManagementService _clientManagementService;

    [SetUp]
    public void Setup()
    {
        _clientRepository = Substitute.For<IClientRepository>();
        _urbaRepository = Substitute.For<IUrbaRepository>();
        _urbaCodesRepository = Substitute.For<IUrbaCodesRepository>();
        _mailService = Substitute.For<IMailService>();
        _logger = Substitute.For<ILogger<ClientManagementService>>();
        _loginStatsRepository = Substitute.For<ILoginStatsRepository>();
        _statsService = Substitute.For<IStatsService>();
        _clientManagementService = new ClientManagementService(_logger, _clientRepository, _urbaRepository, _mailService, _statsService, _loginStatsRepository, _urbaCodesRepository);
    }

    [Test]
    public void AddClient_WhenCalled_ReturnsExpectedResult()
    {
        // Arrange
        var newClient = new CreateDto { Name = "Name", User = "User", Pass = "Pass", Floor = "Floor", Door = "Door", House = "House" };
        var urbaId = 1;

        // Act
        var result = _clientManagementService.AddClient(newClient, urbaId);

        // Assert
        // Assert.IsTrue(result);
    }

    [Test]
    public void CheckUserExist_WhenCalled_ReturnsExpectedResult()
    {
        // Arrange
        var user = "User";
        var pass = "Pass";
        var client = new ClientDb { id = 1, username = user, pass = pass, token = "Token" };
        _clientRepository.GetClientWithUser(user).Returns(client);

        // Act
        var result = _clientManagementService.CheckUserExist(user, pass);

        // Assert
        // Assert.IsTrue(result.Success);
        // Assert.AreEqual(client.token, result.Token);
        // Assert.AreEqual(client.id, result.Id);
    }
}

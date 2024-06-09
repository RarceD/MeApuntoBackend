using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UniTest.Services;

[TestFixture]
public class BookerManagementServiceTests
{
    private IClientRepository _clientRepository;
    private IUrbaRepository _urbaRepository;
    private ISchedulerRepository _schedulerRepository;
    private IConfigurationRepository _configurationRepository;
    private ICourtRepository _courtRepository;
    private ILogger<BookerManagementService> _logger;
    private IMailService _mailService;
    private IBookerStatsRepository _bookerStatsRepository;
    private IBookerService _bookerStrategy;
    private IServiceProvider _serviceProvider;
    // private BookerManagementService? _bookerManagementService;

    [SetUp]
    public void Setup()
    {
        _clientRepository = Substitute.For<IClientRepository>();
        _urbaRepository = Substitute.For<IUrbaRepository>();
        _schedulerRepository = Substitute.For<ISchedulerRepository>();
        _configurationRepository = Substitute.For<IConfigurationRepository>();
        _courtRepository = Substitute.For<ICourtRepository>();
        _logger = Substitute.For<ILogger<BookerManagementService>>();
        _mailService = Substitute.For<IMailService>();
        _bookerStatsRepository = Substitute.For<IBookerStatsRepository>();
        _bookerStrategy = Substitute.For<IBookerService>();
        _serviceProvider = Substitute.For<IServiceProvider>();
        //_serviceProvider.GetRequiredService<IBookerService>().Returns(_bookerStrategy);
        //_bookerManagementService = new BookerManagementService(_clientRepository, _serviceProvider, _urbaRepository, _schedulerRepository, _configurationRepository, _mailService, _courtRepository, _bookerStatsRepository, _logger);
    }

    [Test]
    public void MakeABook_WhenCalled_ReturnsExpectedResult()
    {
        // Arrange
        var newBook = new BookerDto { CourtId = 1, Time = "10:00", Day = "2022-12-31", Duration = DurationType.ONE_HOUR, Id = 1 };
        var client = new ClientDb { id = 1, urba_id = 1 };
        var urba = new UrbaDb { Id = 1 };
        _clientRepository.GetById(newBook.Id).Returns(client);
        _urbaRepository.GetById(client.urba_id).Returns(urba);
        _bookerStrategy.ValidDayHour(newBook, client.id).Returns(true);

        // Act
        // var result = _bookerManagementService.MakeABook(newBook);

        // Assert
        // Assert.That(result, Is.True, "It is not valid Hour");
    }
}

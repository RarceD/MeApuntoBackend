using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Repositories;
using MeApuntoBackend.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UniTest.Services;

[TestFixture]
public class BookerServiceTests
{
    private ISchedulerRepository _schedulerRepository;
    private IConfigurationRepository _configurationRepository;
    private IMailService _mailService;
    private ILogger<BookerService> _logger;
    private BookerService _bookerService;

    [SetUp]
    public void Setup()
    {
        _schedulerRepository = Substitute.For<ISchedulerRepository>();
        _configurationRepository = Substitute.For<IConfigurationRepository>();
        _mailService = Substitute.For<IMailService>();
        _logger = Substitute.For<ILogger<BookerService>>();
        _bookerService = new BookerService(_schedulerRepository, _logger, _configurationRepository, _mailService);
    }

    [Test]
    public void ValidDayHour_WhenCalled_ReturnsExpectedResult()
    {
        // Arrange
        //var newBook = new BookerDto { CourtId = 1, Time = "10:00", Day = "2022-12-31", Duration = DurationType.ONE_HOUR, Id = 1 };
        //var clientId = 1;

        // Act
        // var result = _bookerService.ValidDayHour(newBook, clientId);

        // Assert
        // Assert.That(result, Is.True, "1 should not be prime");
    }
}

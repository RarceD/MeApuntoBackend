using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;

namespace MeApuntoBackend.Services;
public class BookerOneOnlyStrategy : IBookerStategy
{
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IMailService _mailService;
    private readonly ILogger<BookerService> _logger;

    public BookerOneOnlyStrategy(
            ISchedulerRepository schedulerRepository,
            IConfigurationRepository configurationRepository,
            IMailService mailService,
            ILogger<BookerService> logger)
    {
        _schedulerRepository = schedulerRepository;
        _configurationRepository = configurationRepository;
        _mailService = mailService;
        _logger = logger;
    }

    public bool ValidDayHour(BookerDto newBook, int clientId)
    {
        List<ConfigurationDb> validHours = _configurationRepository.GetAllFromCourtId(newBook.CourtId);
        if (validHours.Count() == 0) return false;

        // First check if hour is valid according configuration:
        if (!validHours.Select(i => i.ValidHour).Contains(newBook.Time)) return false;

        // Check if there are more active books for this client:
        var booksBySameUser = _schedulerRepository.GetByClientId(clientId);
        if (booksBySameUser.Count() > 0)
        {
            _logger.LogWarning($"[BOOK] client {newBook.Id} try booking when have a previous book");
            return false;
        }

        // Check someone has previously book same day but other courts:
        var bookThisDayBySameUser = _schedulerRepository.GetBookInDay(newBook.Day ?? string.Empty)
        .Where(b => b.ClientId == clientId).ToList();
        if (bookThisDayBySameUser.Any()) return false;

        // Check someone has previously book same court
        var bookThisDay = _schedulerRepository.GetBookInDay(newBook.Day ?? string.Empty).Where(c => c.CourtId == newBook.CourtId).ToList();
        foreach (var b in bookThisDay)
        {
            // If I have previously book same court break
            if (b.ClientId == newBook.Id)
            {
                _logger.LogWarning($"[BOOK] client {newBook.Id} has book same court for same day");
                return false;
            }
            else
            {
                // TODO: Verify that booking more than one hours works:
                if (newBook.Duration == DurationType.ONE_HALF_HOUR)
                {
                    // Check that then next hour, for example book at 13:00 two fucking hours so 14:00 must be free
                    var hourToVerifyIsFree = GetNextHour(newBook.Time ?? string.Empty);
                    if (bookThisDay.FirstOrDefault(t => t.Time == hourToVerifyIsFree) != null)
                    {
                        return false;
                    }
                    hourToVerifyIsFree = GetNextHour(hourToVerifyIsFree);
                    if (bookThisDay.FirstOrDefault(t => t.Time == hourToVerifyIsFree) != null)
                    {
                        return false;
                    }
                }
                // If other client has previously book same court SAME hour:
                if (b.CourtId == newBook.CourtId && b.Time == newBook.Time)
                {
                    _logger.LogWarning("Other client has book same court for same day");
                    return false;
                }
            }

        }
        return true;
    }

    private static string GetNextHour(string currentTime)
    {
        if (currentTime.Split(":")[1] == "30")
        {
            return (int.Parse(currentTime.Split(":")[0]) + 1).ToString() + ":00";
        }
        else
        {
            return currentTime.Split(":")[0] + ":30";
        }
    }
}

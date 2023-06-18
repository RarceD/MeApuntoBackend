using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using System.Globalization;

namespace MeApuntoBackend.Services;
public class BookerManagementService : IBookerManagementService
{
    #region Constructor

    private readonly IClientRepository _clientRepository;
    private readonly IUrbaRepository _urbaRepository;
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ICourtRepository _courtRepository;
    private readonly ILogger<BookerManagementService> _logger;
    private readonly static CultureInfo spanishCulture = new("es-ES");
    private readonly IMailService _mailService;
    private readonly IStatsService _statsService;

    public BookerManagementService(IClientRepository clientRepository,
          IUrbaRepository urbaRepository,
          ISchedulerRepository schedulerRepository,
          IConfigurationRepository configurationRepository,
          IMailService mailService,
          ICourtRepository courtRepository,
          IStatsService statsService,
          ILogger<BookerManagementService> logger)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
        _schedulerRepository = schedulerRepository;
        _configurationRepository = configurationRepository;
        _courtRepository = courtRepository;
        _logger = logger;
        _mailService = mailService;
        _statsService = statsService;
    }

    #endregion

    public IEnumerable<BookerResponse> GetBooks(int clientId)
    {
        // Get client who books and it's urba:
        ClientDb client = _clientRepository.GetById(clientId);
        UrbaDb urba = _urbaRepository.GetById(client.urba_id);

        // Get courts from this urba:
        foreach (var court in _courtRepository.GetFromUrbaId(urba.Id))
        {
            // Get all valid hours for this court that are visibles:
            var allBooks = _schedulerRepository.GetBooksByCourtId(court.Id).Where(i => i.Show);
            foreach (var book in allBooks)
            {
                DateTime date = GetDateTimeForToday(book);
                string weekday = spanishCulture.DateTimeFormat.GetDayName(date.DayOfWeek);
                yield return new BookerResponse
                {
                    Id = book.Id,
                    ClientId = book.ClientId,
                    CourtName = court.name,
                    ClientName = _clientRepository.GetById(book.ClientId).name ?? "XXX.0.0.0",
                    Duration = book.Duration,
                    Hour = book.Time,
                    Type = court.type,
                    Weekday = weekday.Substring(0, 1).ToUpper() + weekday.Substring(1)
                };
            }
        }
    }

    public bool MakeABook(BookerDto newBook)
    {
        // First check if valid urba advance book:
        ClientDb clienteWhoBook = _clientRepository.GetById(newBook.Id);
        if (clienteWhoBook == null) return false;
        UrbaDb urba = _urbaRepository.GetById(clienteWhoBook.urba_id);
        if (urba == null) return false;

        // TODO: In case at 12:00 two fucking clients try to book same hour/day
        lock (this)
        {
            // Validate time and hour:
            if (!ValidDayHour(newBook)) return false;
            // Finally make the book:
            bool success = MakeBook(newBook, clienteWhoBook.username ?? string.Empty);
            // Update stats from player:
            if (success)
            {
                clienteWhoBook.plays++;
                _clientRepository.Update(clienteWhoBook);
            }
            return success;
        }
    }

    public bool DeleteBook(int clientId, int bookId)
    {
        var scheduler = _schedulerRepository.GetById(bookId);
        if (scheduler.ClientId != clientId)
        {
            _logger.LogError($"[BOOK] client {clientId} it trying to delete other book from client: {scheduler.ClientId}");
            return false;
        }
        try
        {
            _schedulerRepository.Remove(scheduler);
            if (scheduler.Duration == DurationType.ONE_HOUR)
            {
                scheduler = _schedulerRepository.GetById(bookId + 1);
                _schedulerRepository.Remove(scheduler);
            }
            else if (scheduler.Duration == DurationType.ONE_HALF_HOUR)
            {
                scheduler = _schedulerRepository.GetById(bookId + 1);
                _schedulerRepository.Remove(scheduler);
                scheduler = _schedulerRepository.GetById(scheduler.Id + 1);
                _schedulerRepository.Remove(scheduler);
            }
            else if (scheduler.Duration == DurationType.TWO_HOURS)
            {
                // TODO:
            }

            var email = _clientRepository.GetById(clientId).username;
            if (email != null)
            {
                _mailService.SendCanceledEmail(email, scheduler.Day, scheduler.Time, scheduler.Duration);
            }

            var clientWhoNotBook = _clientRepository.GetById(clientId);
            if (clientWhoNotBook != null)
            {
                clientWhoNotBook.plays--;
                _clientRepository.Update(clientWhoNotBook);
            }

            _logger.LogWarning($"[BOOK] clientId:{clientId} has delete book for court:{scheduler.CourtId} - {scheduler.Day} - {scheduler.Time}");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"[BOOK ERROR] clientId:{clientId} has NOT delete for court:{scheduler.CourtId} - {scheduler.Day} - {scheduler.Time}");
            _logger.LogError($"Exception launch:{e.Message}");
            return false;
        }
    }

    #region Private Method
    private bool ValidDayHour(BookerDto newBook)
    {
        List<ConfigurationDb> validHours = _configurationRepository.GetAllFromCourtId(newBook.CourtId);
        if (validHours.Count() == 0) return false;

        // First check if hour is valid according configuration:
        if (!validHours.Select(i => i.ValidHour).Contains(newBook.Time)) return false;

        // Check someone has previously book same hour
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

    private bool MakeBook(BookerDto book, string emailToSend)
    {
        SchedulerDb newBook = ConvertBookerToScheduler(book);
        try
        {
            // TODO: this is a piece of shit and should be refactor:
            _schedulerRepository.Add(newBook);
            if (newBook.Duration == DurationType.ONE_HOUR)
            {
                // Second not visible 30 min after:
                newBook = ConvertBookerToScheduler(book);
                newBook.Show = false;
                newBook.Time = GetNextHour(newBook.Time ?? string.Empty);
                _schedulerRepository.Add(newBook);
            }
            else if (newBook.Duration == DurationType.ONE_HALF_HOUR)
            {
                // Second not visible 30 min after:
                newBook = ConvertBookerToScheduler(book);
                newBook.Show = false;
                newBook.Time = GetNextHour(newBook.Time ?? string.Empty);
                _schedulerRepository.Add(newBook);

                // Second not visible 30 min after:
                newBook = ConvertBookerToScheduler(book);
                newBook.Show = false;
                newBook.Time = GetNextHour(GetNextHour(newBook.Time ?? string.Empty));
                _schedulerRepository.Add(newBook);
            }
            else if (newBook.Duration == DurationType.TWO_HOURS)
            {
                // TODO
            }

            _mailService.SendConfirmationEmail(emailToSend, newBook.Day, newBook.Time, newBook.Duration);
            _logger.LogWarning($"[BOOK] ClientId:{newBook.ClientId} has book for {book.CourtId} - {book.Time} - {book.Day}");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"[BOOK] ClientId:{newBook.ClientId} has NOT book for {newBook.CourtId} - {newBook.Time} - {newBook.Day}");
            _logger.LogError($"[BOOK] Exception launch:{e.Message}");
            return false;
        }
    }

    private static SchedulerDb ConvertBookerToScheduler(BookerDto book)
    {
        return new SchedulerDb()
        {
            ClientId = book.Id,
            CourtId = book.CourtId,
            Time = book.Time,
            Day = book.Day,
            Duration = book.Duration,
            Show = true
        };
    }
    private static DateTime GetDateTimeForToday(SchedulerDb book)
    {
        DateTime date = new();
        // This shit is necessary because of linux/windows changes on time formatting
        try
        {
            date = DateTime.ParseExact(book.Day ?? string.Empty, "MM/dd/yyyy", null);
        }
        catch
        {
            date = DateTime.Parse(book.Day ?? string.Empty);
        }

        return date;
    }

    #endregion
}

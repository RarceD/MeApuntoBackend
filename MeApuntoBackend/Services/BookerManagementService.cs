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
    private static CultureInfo spanishCulture = new CultureInfo("es-ES");
    public BookerManagementService(IClientRepository clientRepository,
          IUrbaRepository urbaRepository,
          ISchedulerRepository schedulerRepository,
          IConfigurationRepository configurationRepository,
          ICourtRepository courtRepository,
          ILogger<BookerManagementService> logger)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
        _schedulerRepository = schedulerRepository;
        _configurationRepository = configurationRepository;
        _courtRepository = courtRepository;
        _logger = logger;
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
            // Get all valid hours for this court:
            var allBooks = _schedulerRepository.GetBooksByCourtId(court.Id);
            foreach (var book in allBooks)
            {
                DateTime date = DateTime.ParseExact(book.Day ?? string.Empty, "dd/MM/yyyy", null);
                yield return new BookerResponse
                {
                    Id = book.Id,
                    ClientId = book.ClientId,
                    CourtName = court.name,
                    ClientName = _clientRepository.GetById(book.ClientId).name,
                    Duration = book.Duration,
                    Hour = book.Time,
                    Type = court.type,
                    Weekday = spanishCulture.DateTimeFormat.GetDayName(date.DayOfWeek)
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

        // Validate time and hour:
        if (!ValidDayHour(newBook.Day ?? string.Empty, newBook.Time ?? string.Empty, urba.advance_book, newBook.Id)) return false;

        // Finally make the book:
        return MakeBook(clienteWhoBook.id, newBook.CourtId, newBook.Time ?? string.Empty, newBook.Day ?? string.Empty);
    }

    public bool DeleteBook(int clientId, int bookId)
    {
        var scheduler = _schedulerRepository.GetById(bookId);
        try
        {
            _schedulerRepository.Remove(scheduler);
            string logString = $"ClientId:{clientId} has delete book for {scheduler.CourtId} - {scheduler.Day} - {scheduler.Time}";
            _logger.LogWarning(logString);
            return true;
        }
        catch (Exception e)
        {
            string logString = $"ClientId:{clientId} has NOT delete  for {scheduler.CourtId} - {scheduler.Day} - {scheduler.Time}";
            _logger.LogWarning(logString);
            _logger.LogError($"Exception launch:{e.Message}");
            return false;
        }
    }

    private List<BookSchedul> CheckHoursAreValidToBook(List<ConfigurationDb> hours, int clientId, int advanceBook)
    {
        List<BookSchedul> bookScheduls = new List<BookSchedul>();
        for (int d = 0; d <= advanceBook; d++)
        {
            var day = DateTime.Now.AddDays(d).Date.ToShortDateString();

            // Get all the books for the day x
            foreach (var h in hours)
                bookScheduls.Add(new BookSchedul()
                {
                    Available = true,
                    HourAvailable = h.ValidHour,
                    Day = day
                });

            // Check if they are really not valid:
            List<SchedulerDb> invalidHours = _schedulerRepository.GetBookInDay(day);
            foreach (var b in bookScheduls)
            {
                if (invalidHours.Select(i => i.Time).Contains(b.HourAvailable))
                    b.Available = false;
            }
        }
        return bookScheduls;
    }

    private bool ValidDayHour(string dayToBook, string hourToBook, int courtId, int clientId)
    {
        List<ConfigurationDb> validHours = _configurationRepository.GetAllFromCourtId(courtId);
        if (validHours.Count() == 0) return false;

        // First check if hour is valid according configuration:
        if (!validHours.Select(i => i.ValidHour).Contains(hourToBook)) return false;

        // Check someone has previously book same hour
        var bookThisDay = _schedulerRepository.GetBookInDay(dayToBook);
        foreach (var b in bookThisDay)
        {
            // If I have previously book same court break
            if (b.ClientId == clientId)
            {
                _logger.LogWarning("This client has book same court for same day");
                if (b.CourtId == courtId)
                    return false;
            }
            else
            {
                // If other client has previously book same court SAME hour:
                _logger.LogWarning("Other client has book same court for same day");
                if (b.CourtId == courtId && b.Time == hourToBook)
                    return false;
            }

        }
        return true;
    }
    private bool MakeBook(int clientId, int courtId, string hourToBook, string dayToBook)
    {
        var newBook = new SchedulerDb() { ClientId = clientId, CourtId = courtId, Time = hourToBook, Day = dayToBook };
        try
        {
            _schedulerRepository.Add(newBook);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"ClientId:{clientId} has NOT book for {courtId} - {dayToBook} - {hourToBook}");
            _logger.LogError($"Exception launch:{e.Message}");
            return false;
        }
    }
}

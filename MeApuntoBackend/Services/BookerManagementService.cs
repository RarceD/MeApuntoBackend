using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;

namespace MeApuntoBackend.Services;
public class BookerManagementService : IBookerManagementService
{
    #region Constructor

    private readonly IClientRepository _clientRepository;
    private readonly IUrbaRepository _urbaRepository;
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ICourtRepository _courtRepository;
    private readonly IBookerRepository _bookerRepository;
    public BookerManagementService(IClientRepository clientRepository,
          IUrbaRepository urbaRepository,
          ISchedulerRepository schedulerRepository,
          IConfigurationRepository configurationRepository,
            IBookerRepository bookerRepository,
          ICourtRepository courtRepository)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
        _schedulerRepository = schedulerRepository;
        _configurationRepository = configurationRepository;
        _courtRepository = courtRepository;
        _bookerRepository = bookerRepository;
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
            var validHours = _configurationRepository.GetAllFromCourtId(court.Id).ToList();
            yield return new BookerResponse
            {
                CourtId = court.Id,
                CourtName = court.name,
                Scheduler = CheckHoursAreValidToBook(validHours, client.id, urba.advance_book)
            };
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
        return true;
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
        foreach(var b in bookThisDay)
        {
            if (b.Time == hourToBook) return false;
        }

        // Check this client has book same day:
        var previousClientBooks = _bookerRepository.GetFromClientId(clientId);
        if (previousClientBooks.Count() == 0) return true;
        foreach (var prevBooks in previousClientBooks)
        {
            if (prevBooks.Day == dayToBook) return false;
        }

        return true;
    }
    private bool MakeBook(int clientId, int courtId, string hourToBook, string dayToBook)
    {
        var bookDay = Convert.ToDateTime(dayToBook);
        var newBook = new SchedulerDb() { ClientId = clientId, CourtId = courtId, Time = hourToBook, Day = dayToBook };
        var newRegister = new BookerDb() { client_id = clientId, court_id = courtId, Day = dayToBook, weekday = (int)bookDay.DayOfWeek, duration = "1", time_book = hourToBook };
        try
        {
            _bookerRepository.Add(newRegister);
            _schedulerRepository.Add(newBook);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

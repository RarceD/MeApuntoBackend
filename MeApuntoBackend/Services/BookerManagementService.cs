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
    public BookerManagementService(IClientRepository clientRepository,
          IUrbaRepository urbaRepository,
          ISchedulerRepository schedulerRepository,
          IConfigurationRepository configurationRepository,
          ICourtRepository courtRepository)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
        _schedulerRepository = schedulerRepository;
        _configurationRepository = configurationRepository;
        _courtRepository = courtRepository;
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

        // Validate time:
        DateTime bookTime = DateTime.Now;
        DateTime.TryParse(newBook.Time, out bookTime);
        if (!ValidAdvanceBookDay(bookTime, urba.advance_book)) return false;
        if (!ValidHour(bookTime, newBook.CourtId)) return false;

        // Finally make the book:
        return MakeBook(clienteWhoBook.id, newBook.CourtId, newBook.Time ?? string.Empty);
    }

    public bool DeleteBook(int clientId, int bookId)
    {
        return true;
    }

    private List<BookSchedul> CheckHoursAreValidToBook(List<ConfigurationDb> hours, int clientId, int advanceBook)
    {
        List<BookSchedul> bookScheduls = new List<BookSchedul>();
        // Now detect the unvalid hours:
        for (int d = 0; d <= advanceBook; d++)
        {
            // Day to check:
            var day = DateTime.Now.AddDays(d);

            // Get all the books for the day x
            foreach (var h in hours)
                bookScheduls.Add(new BookSchedul()
                {
                    Available = true,
                    HourAvailable = h.ValidHour,
                    Day = day
                });
            // Check if they are really not valid:
            // TODO
            foreach (var b in bookScheduls)
            {

            }

        }
        return bookScheduls;
    }

    private bool ValidAdvanceBookDay(DateTime dayToBook, int advanceBook)
    {
        return true;
    }
    private bool ValidHour(DateTime dayToBook, int courtId)
    {
        List<ConfigurationDb> validHours = _configurationRepository.GetAllFromCourtId(courtId);
        if (validHours.Count() == 0) return false;

        if (validHours.Select(i => i.ValidHour).Contains(dayToBook.ToString()))
        {
            return true;
        }
        return true;
    }
    private bool MakeBook(int clientId, int courtId, string date)
    {
        var newBook = new SchedulerDb() { ClientId = clientId, CourtId = courtId, Time = date };
        try
        {
            _schedulerRepository.Add(newBook);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

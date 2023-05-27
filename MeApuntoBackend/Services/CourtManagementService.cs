using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;

namespace MeApuntoBackend.Services;
public class CourtManagementService : ICourtManagementService
{
    #region Contructor

    private readonly IClientRepository _clientRepository;
    private readonly IUrbaRepository _urbaRepository;
    private readonly INormativeRepository _normativeRespository;
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly ICourtRepository _courtRespository;
    private readonly IConfigurationRepository _configurationRepository;

    public CourtManagementService(
        IClientRepository clientRepository,
        IUrbaRepository urbaRepository,
        ISchedulerRepository schedulerRepository,
        INormativeRepository normativeRespository,
        IConfigurationRepository configurationRepository,
        ICourtRepository courtRespository)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
        _normativeRespository = normativeRespository;
        _courtRespository = courtRespository;
        _configurationRepository = configurationRepository;
        _schedulerRepository = schedulerRepository;
    }

    #endregion
    public IEnumerable<CourtResponse> GetCourts(int clientId)
    {
        var courts = new List<CourtResponse>();

        // First get the urbaId:
        var client = _clientRepository.GetById(clientId);
        if (client == null) return courts;

        // Get courts:
        var courtsDb = _courtRespository.GetFromUrbaId(client.urba_id);
        if (courtsDb == null) return courts;

        var urbaBd = _urbaRepository.GetById(client.urba_id);
        if (urbaBd == null) return courts;

        return ConvertToCourtResponse(courtsDb, urbaBd.advance_book);
    }
    private IEnumerable<CourtResponse> ConvertToCourtResponse(List<CourtDb> courtsDb, int advanceBook)
    {
        foreach (var c in courtsDb)
        {
            yield return new CourtResponse()
            {
                Id = c.Id,
                Name = c.name,
                Timetables = GetTimetablesFromCourt(c.Id, advanceBook),
                Type = c.type,
                ValidTimes = c.valid_times
            };
        }
    }
    public class TimeComparer : IComparer<string>
    {
        int IComparer<string>.Compare(string? x, string? y)
        {
            int splitX = int.Parse(x.Split(':')[0]);
            int splitY = int.Parse(y.Split(':')[0]);
            if (splitX > splitY) return 1;
            if (splitY > splitX) return -1;
            return 0;
        }
    }

    private List<CourtResponse.Timetable> GetTimetablesFromCourt(int courtId, int advanceBook)
    {
        var allTimetables = new List<CourtResponse.Timetable>();
        int today = 0;
        while (today < advanceBook)
        {
            var t = DateTime.Now.AddDays(today++);
            List<ConfigurationDb> timeDb = _configurationRepository.GetAllFromCourtId(courtId).ToList();
            if (timeDb == null) break;

            // order the maybe not ordered times:
            var tempOrder = timeDb.Select(i => i.ValidHour).ToList();
            tempOrder.Sort(new TimeComparer());

            var day = new CourtResponse.Timetable() { Day = t.Day };
            day.Availability = new List<CourtResponse.TimeAvailability>();
            day.fullDay = t.Date.ToShortDateString();
            foreach (var c in tempOrder)
            {
                day.Availability.Add(new CourtResponse.TimeAvailability()
                {
                    Time = c,// c.ValidHour,
                    Valid = true
                });
            }
            allTimetables.Add(day);

        }
        CheckAvailability(allTimetables, courtId);
        return allTimetables;
    }

    private void CheckAvailability(List<CourtResponse.Timetable> allTimetables, int courtId)
    {
        // TODO: Check performance of this shit:
        allTimetables.ForEach((timetable) =>
        {
            if (timetable.fullDay != null && timetable.Availability != null)
            {
                var allBooksThisDay = _schedulerRepository.GetBookInDay(timetable.fullDay).Where(t => t.CourtId == courtId).ToList();
                var allTimes = timetable.Availability;
                var match = allBooksThisDay.Join(allTimes, books => books.Time, days => days.Time,
                                                    (books, days) => new
                                                    {
                                                        books.Time
                                                    })
                                            .ToList();
                foreach (var m in match)
                {
                    var t = timetable.Availability.Where(t => t.Time == m.Time).FirstOrDefault();
                    if (t == null) continue;
                    t.Valid = false;
                }
            }
        });
    }

    public IEnumerable<NormativeResponse> GetNormativeByClientId(int clientId)
    {
        var normative = new List<NormativeResponse>();

        // First get the urbaId:
        var client = _clientRepository.GetById(clientId);
        if (client == null) return normative;

        // Get normative
        var norms = _normativeRespository.GetAllFromUrbaId(client.urba_id);
        if (norms == null) return normative;
        return convertToDto(norms);
    }

    private IEnumerable<NormativeResponse> convertToDto(IEnumerable<NormativeDb> normatives)
    {
        foreach (var normativeDb in normatives)
        {
            yield return new NormativeResponse()
            {
                Id = normativeDb.Id,
                Text = normativeDb.Text,
                Title = normativeDb.Title
            };
        }

    }

}

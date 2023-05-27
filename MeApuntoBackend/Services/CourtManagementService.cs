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
    private readonly ICourtRepository _courtRespository;
    private readonly IConfigurationRepository _configurationRepository;

    public CourtManagementService(
        IClientRepository clientRepository,
        IUrbaRepository urbaRepository,
        INormativeRepository normativeRespository,
        IConfigurationRepository schedulerRepository,
        ICourtRepository courtRespository)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
        _normativeRespository = normativeRespository;
        _courtRespository = courtRespository;
        _configurationRepository = schedulerRepository;
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
    private List<CourtResponse.Timetable> GetTimetablesFromCourt(int courtId, int advanceBook)
    {
        var allTimetables = new List<CourtResponse.Timetable>();
        int today = 0;
        while (today < advanceBook)
        {
            var t = DateTime.Now.AddDays(today++);
            var timeDb = _configurationRepository.GetAllFromCourtId(courtId);
            if (timeDb == null) break;

            var day = new CourtResponse.Timetable() { Day = t.Day };
            day.Availability = new List<CourtResponse.TimeAvailability>();
            foreach (var c in timeDb)
            {
                day.Availability.Add(new CourtResponse.TimeAvailability()
                {
                    Time = c.ValidHour,
                    Valid = true
                });
            }
            allTimetables.Add(day);

        }
        return allTimetables;
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

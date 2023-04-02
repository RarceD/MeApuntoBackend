using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using System.Collections.Generic;

namespace MeApuntoBackend.Services;
public class CourtManagementService : ICourtManagementService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUrbaRepository _urbaRepository;
    private readonly INormativeRepository _normativeRespository;
    private readonly ICourtRepository _courtRespository;
    public CourtManagementService(
        IClientRepository clientRepository,
        IUrbaRepository urbaRepository,
        INormativeRepository normativeRespository,
        ICourtRepository courtRespository)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
        _normativeRespository = normativeRespository;
        _courtRespository = courtRespository;
    }
    public IEnumerable<CourtResponse> GetCourts(int clientId)
    {
        var courts = new List<CourtResponse>();

        // First get the urbaId:
        var client = _clientRepository.GetById(clientId);
        if (client == null) return courts;

        // Get courts:
        //var courtsDb = _courtRespository.(client.urba_id);
        //if (courtsDb == null) return courts;
        return courts;
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
        var normative = new List<NormativeResponse>();
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

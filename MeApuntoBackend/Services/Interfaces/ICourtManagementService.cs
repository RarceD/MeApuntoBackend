using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services;
public interface ICourtManagementService
{
    IEnumerable<NormativeResponse> GetNormativeByClientId(int clientId);
    IEnumerable<CourtResponse> GetCourts(int clientId);
}

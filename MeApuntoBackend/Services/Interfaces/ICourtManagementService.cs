using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services;
public interface ICourtManagementService
{
    IEnumerable<NormativeResponse> GetNormativeByClientId(int clientId);
}

using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public interface IConfigurationRepository : IRepository<ConfigurationDb>
{
    List<ConfigurationDb> GetAllFromCourtId(int courtId);
}

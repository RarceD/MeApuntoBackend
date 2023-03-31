using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public interface INormativeRepository : IRepository<NormativeDb>
{
    IEnumerable<NormativeDb> GetAllFromUrbaId(int id);
}

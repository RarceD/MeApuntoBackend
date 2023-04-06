using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public interface ICourtRepository : IRepository<CourtDb>
{
    List<CourtDb> GetFromUrbaId(int urbaId);
}

using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public interface IBookerRepository : IRepository<BookerDb>
{
    List<BookerDb> GetFromClientId(int clientId);
}

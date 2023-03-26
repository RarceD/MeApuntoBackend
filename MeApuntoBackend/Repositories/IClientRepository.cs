using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public interface IClientRepository : IRepository<ClientDb>
{
    ClientDb? GetClientWithUser(string user);
    bool IsValidUserCode(string code);
}

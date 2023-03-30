using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public interface IUrbaRepository : IRepository<UrbaDb>
{
    UrbaDb? GetUrbatWithToken(string token);
}

using MeApuntoBackend.Models;

namespace MeApuntoBackend.Repositories;
public interface ISchedulerRepository : IRepository<SchedulerDb>
{
    List<SchedulerDb> GetBookInDay(string day);
}

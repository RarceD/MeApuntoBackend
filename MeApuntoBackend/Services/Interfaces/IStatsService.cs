
using MeApuntoBackend.Services.Dto;

namespace MeApuntoBackend.Services;
public interface IStatsService
{
    void AddLoginRecord(LoginRecord success);
    void AddBookerRecord(BookerRecord success);
}

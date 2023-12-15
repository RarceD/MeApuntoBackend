using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services;
public interface IBookerStategy
{
    bool ValidDayHour(BookerDto newBook, int clientId);
}

using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services;
public interface IBookerStategy
{
    bool MakeABook(BookerDto newBook, string emailToSend);
    bool ValidDayHour(BookerDto newBook, int clientId);
}

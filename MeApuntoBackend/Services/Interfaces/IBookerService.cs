using MeApuntoBackend.Controllers.Dtos;
namespace MeApuntoBackend.Services;
public enum BookerStategy
{
    MAIN = 0,
    ONE_BOOK_ONLY
};

public interface IBookerService
{
    void SetStrategy(BookerStategy strategy);
    bool MakeABook(BookerDto newBook, string emailToSend);
    bool ValidDayHour(BookerDto newBook, int clientId);
}

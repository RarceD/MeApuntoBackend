using MeApuntoBackend.Controllers.Dtos;
namespace MeApuntoBackend.Services;
public enum BookerStategy
{
    MAIN,
    SPECIFIC
};

public interface IBookerService
{
    void SetStrategy(BookerStategy strategy);
    bool MakeABook(BookerDto newBook, string emailToSend);
    bool ValidDayHour(BookerDto newBook, int clientId);
}

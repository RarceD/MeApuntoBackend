using MeApuntoBackend.Controllers.Dtos;

namespace MeApuntoBackend.Services;
public interface IBookerManagementService
{
    bool MakeABook(BookerDto newBook);
    IEnumerable<BookerResponse> GetBooks(int clientId);
    bool DeleteBook(int clientId, int bookId);
}

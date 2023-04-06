using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : GenericController
{
    private readonly ILogger<LoginController> _logger;
    private readonly IBookerManagementService _bookerManagementService;
    public BookController(ILogger<LoginController> logger,
            IClientManagementService loginManagementService,
            IBookerManagementService bookerManagementService)
        : base(loginManagementService)
    {
        _logger = logger;
        _bookerManagementService = bookerManagementService;
    }

    [HttpGet]
    public IEnumerable<BookerResponse> GetBooks(string token, int id)
    {
        var response = new List<BookerResponse>();
        if (!CheckUserTokenId(token, id)) return response;
        var allBooks = _bookerManagementService.GetBooks(id);
        if (allBooks == null) return response;
        return allBooks;
    }

    [HttpPost]
    public ActionResult MakeBook(BookerDto input)
    {
        if (!CheckUserTokenId(input.Token ?? string.Empty, input.Id)) return new NoContentResult();
        bool success = _bookerManagementService.MakeABook(input);
        return success ? Ok() : NoContent();
    }

    [HttpPost("delete")]
    public ActionResult Delte(BookerDto input)
    {
        if (!CheckUserTokenId(input.Token ?? string.Empty, input.Id))  return NoContent(); 
        int bookId = 123;
        var allBooks = _bookerManagementService.DeleteBook(input.Id, bookId);
        return true ? Ok() : NoContent();
    }
}
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
        if (success)
        {
            _logger.LogWarning($"ClientId: {input.Id} has successfully make a book in courtId:{input.CourtId} for {input.Time}-{input.Day}");
            return Ok();
        }
        else
        {
            _logger.LogWarning($"ClientId: {input.Id} has ERROR making book courtId:{input.CourtId} for {input.Time}-{input.Day}");
            return NoContent();
        }
    }

    [HttpPost("delete")]
    public ActionResult Delete(BookerDto input)
    {
        if (!CheckUserTokenId(input.Token ?? string.Empty, input.Id)) return NoContent();
        int bookId = 123;
        var success = _bookerManagementService.DeleteBook(input.Id, bookId);
        if (success)
        {
            _logger.LogWarning($"ClientId: {input.Id} delete book for courtId:{input.CourtId} for {input.Time}-{input.Day}");
        }
        else
        {
            _logger.LogError($"ClientId: {input.Id} ERROR deleting book for courtId:{input.CourtId} for {input.Time}-{input.Day}");
        }

        return success ? Ok() : NoContent();
    }
}
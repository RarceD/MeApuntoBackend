using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using System.Globalization;

namespace MeApuntoBackend.Services;
public class BookerService : IBookerService
{
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ILogger<BookerService> _logger;
    private readonly IMailService _mailService;
    private IBookerStategy? _bookerStategy;

    public BookerService(
          ISchedulerRepository schedulerRepository,
          ILogger<BookerService> logger,
          IConfigurationRepository configurationRepository,
          IMailService mailService)
    {
        _schedulerRepository = schedulerRepository;
        _configurationRepository = configurationRepository;
        _mailService = mailService;
        _logger = logger;
    }

    public void SetStrategy(BookerStategy strategy)
    {
        switch (strategy)
        {
            case BookerStategy.MAIN:
                _bookerStategy = new BookerMainStrategy(
                                    _schedulerRepository,
                                    _configurationRepository,
                                    _mailService,
                                    _logger);
                break;

            case BookerStategy.SPECIFIC:
                // TODO
                break;
        }
    }

    public bool MakeABook(BookerDto newBook, string emailToSend)
    {
        if (_bookerStategy == null) throw new InvalidOperationException();
        return _bookerStategy.MakeABook(newBook, emailToSend);
    }

    public bool ValidDayHour(BookerDto newBook, int clientId)
    {
        if (_bookerStategy == null) throw new InvalidOperationException();
        return _bookerStategy.ValidDayHour(newBook, clientId);
    }
}

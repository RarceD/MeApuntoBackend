﻿using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Repositories;

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

        // By default I set the most liked booking way 
        SetStrategy(BookerStategy.MAIN);
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

            case BookerStategy.ONE_BOOK_ONLY:
                _bookerStategy = new BookerOneOnlyStrategy(
                                    _schedulerRepository,
                                    _configurationRepository,
                                    _mailService,
                                    _logger);
                break;
        }
    }
    public bool ValidDayHour(BookerDto newBook, int clientId)
    {
        if (_bookerStategy == null) throw new InvalidOperationException();
        return _bookerStategy.ValidDayHour(newBook, clientId);
    }
}

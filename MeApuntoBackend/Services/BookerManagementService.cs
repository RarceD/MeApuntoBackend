﻿using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using System.Globalization;

namespace MeApuntoBackend.Services;
public class BookerManagementService : IBookerManagementService
{
    #region Constructor

    private readonly IClientRepository _clientRepository;
    private readonly IUrbaRepository _urbaRepository;
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ICourtRepository _courtRepository;
    private readonly ILogger<BookerManagementService> _logger;
    private static CultureInfo spanishCulture = new CultureInfo("es-ES");
    public BookerManagementService(IClientRepository clientRepository,
          IUrbaRepository urbaRepository,
          ISchedulerRepository schedulerRepository,
          IConfigurationRepository configurationRepository,
          ICourtRepository courtRepository,
          ILogger<BookerManagementService> logger)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
        _schedulerRepository = schedulerRepository;
        _configurationRepository = configurationRepository;
        _courtRepository = courtRepository;
        _logger = logger;
    }

    #endregion

    public IEnumerable<BookerResponse> GetBooks(int clientId)
    {
        // Get client who books and it's urba:
        ClientDb client = _clientRepository.GetById(clientId);
        UrbaDb urba = _urbaRepository.GetById(client.urba_id);

        // Get courts from this urba:
        foreach (var court in _courtRepository.GetFromUrbaId(urba.Id))
        {
            // Get all valid hours for this court:
            var allBooks = _schedulerRepository.GetBooksByCourtId(court.Id);
            foreach (var book in allBooks)
            {
                DateTime date = DateTime.ParseExact(book.Day ?? string.Empty, "dd/MM/yyyy", null);
                yield return new BookerResponse
                {
                    Id = book.Id,
                    ClientId = book.ClientId,
                    CourtName = court.name,
                    ClientName = _clientRepository.GetById(book.ClientId).name,
                    Duration = book.Duration,
                    Hour = book.Time,
                    Type = court.type,
                    Weekday = spanishCulture.DateTimeFormat.GetDayName(date.DayOfWeek)
                };
            }
        }
    }

    public bool MakeABook(BookerDto newBook)
    {
        // First check if valid urba advance book:
        ClientDb clienteWhoBook = _clientRepository.GetById(newBook.Id);
        if (clienteWhoBook == null) return false;
        UrbaDb urba = _urbaRepository.GetById(clienteWhoBook.urba_id);
        if (urba == null) return false;

        // Validate time and hour:
        if (!ValidDayHour(newBook)) return false;

        // Finally make the book:
        return MakeBook(newBook);
    }

    public bool DeleteBook(int clientId, int bookId)
    {
        var scheduler = _schedulerRepository.GetById(bookId);
        if (scheduler.ClientId != clientId)
        {
            _logger.LogError($"[BOOK] client {clientId} it trying to delete other book from client: {scheduler.ClientId}");
            return false;
        }
        try
        {
            _schedulerRepository.Remove(scheduler);
            string logString = $"[BOOK] clientId:{clientId} has delete book for court:{scheduler.CourtId} - {scheduler.Day} - {scheduler.Time}";
            _logger.LogWarning(logString);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"[BOOK] clientId:{clientId} has NOT delete for court:{scheduler.CourtId} - {scheduler.Day} - {scheduler.Time}");
            _logger.LogError($"Exception launch:{e.Message}");
            return false;
        }
    }
    private bool ValidDayHour(BookerDto newBook)
    {
        List<ConfigurationDb> validHours = _configurationRepository.GetAllFromCourtId(newBook.CourtId);
        if (validHours.Count() == 0) return false;

        // First check if hour is valid according configuration:
        if (!validHours.Select(i => i.ValidHour).Contains(newBook.Time)) return false;

        // Check someone has previously book same hour
        var bookThisDay = _schedulerRepository.GetBookInDay(newBook.Day);
        foreach (var b in bookThisDay)
        {
            // If I have previously book same court break
            if (b.ClientId == newBook.Id)
            {
                _logger.LogWarning($"[BOOK] client {newBook.Id} has book same court for same day");
                if (b.CourtId == newBook.Id)
                    return false;
            }
            else
            {
                // If other client has previously book same court SAME hour:
                _logger.LogWarning("Other client has book same court for same day");
                if (b.CourtId == newBook.CourtId && b.Time == newBook.Time)
                    return false;
            }

        }
        return true;
    }
    private bool MakeBook(BookerDto book)
    {
        var newBook = new SchedulerDb()
        {
            ClientId = book.Id,
            CourtId = book.CourtId,
            Time = book.Time,
            Day = book.Day,
            Duration = book.Duration
        };
        try
        {
            _schedulerRepository.Add(newBook);
            _logger.LogError($"[BOOK] ClientId:{newBook.ClientId} has book for {newBook.CourtId} - {newBook.Time} - {newBook.Day}");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"[BOOK] ClientId:{newBook.ClientId} has NOT book for {newBook.CourtId} - {newBook.Time} - {newBook.Day}");
            _logger.LogError($"[BOOK] Exception launch:{e.Message}");
            return false;
        }
    }
}

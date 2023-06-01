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
    private readonly IMailService _mailService;
    public BookerManagementService(IClientRepository clientRepository,
          IUrbaRepository urbaRepository,
          ISchedulerRepository schedulerRepository,
          IConfigurationRepository configurationRepository,
          IMailService mailService,
          ICourtRepository courtRepository,
          ILogger<BookerManagementService> logger)
    {
        _clientRepository = clientRepository;
        _urbaRepository = urbaRepository;
        _schedulerRepository = schedulerRepository;
        _configurationRepository = configurationRepository;
        _courtRepository = courtRepository;
        _logger = logger;
        _mailService = mailService;
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
                DateTime date = GetDateTimeForToday(book);
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

        // TODO: In case at 12:00 two fucking clients try to book same hour/day
        lock (this)
        {
            // Validate time and hour:
            if (!ValidDayHour(newBook)) return false;
            // Finally make the book:
            return MakeBook(newBook, clienteWhoBook.username);
        }
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

            var email = _clientRepository.GetById(clientId).username;
            if (email != null)
            {
                SendDeniedEmail(email);
            }

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

    #region Private Method
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
                // TODO: Verify that booking more than one hours works:
                if (newBook.Duration != DurationType.ONE_HOUR.ToString())
                {
                    // Check that then next hour, for example book at 13:00 two fucking hours so 14:00 must be free
                    var hourToVerifyIsFree = (int.Parse(newBook.Time.Split(":")[0]) + 1).ToString() + ":00";
                    if (bookThisDay.FirstOrDefault(t => t.Time == hourToVerifyIsFree) != null)
                    {
                        return false;
                    }
                }
                // If other client has previously book same court SAME hour:
                if (b.CourtId == newBook.CourtId && b.Time == newBook.Time)
                {
                    _logger.LogWarning("Other client has book same court for same day");
                    return false;
                }
            }

        }
        return true;
    }
    private bool MakeBook(BookerDto book, string emailToSend)
    {
        SchedulerDb newBook = ConvertBookerToScheduler(book);
        try
        {

            if (newBook.Duration == ((int)DurationType.ONE_HOUR).ToString())
            {
                _schedulerRepository.Add(newBook);
            }
            else
            {
                // TODO: Must add ONE register BUT the occupied two hours must be done...
                book.Duration = ((int)DurationType.ONE_HOUR).ToString();
                newBook.Duration = ((int)DurationType.ONE_HOUR).ToString();
                _schedulerRepository.Add(newBook);
                book.Time = (int.Parse(book.Time.Split(":")[0]) + 1).ToString() + ":00";
                _schedulerRepository.Add(ConvertBookerToScheduler(book));
            }

            SendConfirmationEmail(emailToSend);
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

    private void SendConfirmationEmail(string emailToSend)
    {
        try
        {
            // TODO : Multi line string in emails
            // https://stackoverflow.com/questions/22067168/send-multiple-textbox-values-in-mail-body-using-smtp
            string title = "[MEAPUNTO.ONLINE] Reserva de Pista";
            string content = "Su reserva ha sido llevada a cabo con exito.Disfrute de su partida.";
            _mailService.SendEmail(emailToSend, title, content);
        }
        catch (Exception e)
        {
            _logger.LogWarning($"[BOOK] Email ERROR" + e.Message);
        }
    }
    private void SendDeniedEmail(string emailToSend)
    {
        try
        {
            // TODO : Multi line string in emails
            string title = "[MEAPUNTO.ONLINE] Cancelación Reserva";
            string content = "Saludos, su reserva ha sido cancelada con éxito. Un saludo";
            _mailService.SendEmail(emailToSend, title, content);
        }
        catch (Exception e)
        {
            _logger.LogWarning($"[BOOK] Email ERROR" + e.Message);
        }
    }

    private static SchedulerDb ConvertBookerToScheduler(BookerDto book)
    {
        return new SchedulerDb()
        {
            ClientId = book.Id,
            CourtId = book.CourtId,
            Time = book.Time,
            Day = book.Day,
            Duration = book.Duration
        };
    }
    private static DateTime GetDateTimeForToday(SchedulerDb book)
    {
        DateTime date = new();
        // This shit is necessary because of linux/windows changes on time formatting
        try
        {
            date = DateTime.ParseExact(book.Day ?? string.Empty, "dd/MM/yyyy", null);
        }
        catch
        {
            date = DateTime.Parse(book.Day ?? string.Empty);
        }

        return date;
    }

    #endregion
}

using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Models;
using MeApuntoBackend.Repositories;
using System.Net.Mail;

namespace MeApuntoBackend.Services;
public class MailService : IMailService
{
    private MailMessage? _mailMessage;
    private SmtpClient? _smtpClient;
    private const string EMAIL_SOURCE = "no-reply@meapunto.online";
    private const string HOST = "localhost";

    public bool SendEmail(string toMailAddress, string title, string content)
    {
        _mailMessage = new MailMessage();
        _smtpClient = new SmtpClient();

        _mailMessage.From = new MailAddress(EMAIL_SOURCE, EMAIL_SOURCE, System.Text.Encoding.UTF8);
        _mailMessage.To.Add(new MailAddress(toMailAddress));
        _mailMessage.Subject = title;
        _mailMessage.Body = content;
        _mailMessage.IsBodyHtml = true;
        _smtpClient.Host = HOST;
        try
        {
            _smtpClient.Send(_mailMessage);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public bool SendConfirmationEmail(string toMailAddress, string content)
    {
        return SendEmail(toMailAddress, "Confirmación de reserva", content);
    }
    public bool SendCanceledEmail(string toMailAddress, string content)
    {
        return SendEmail(toMailAddress, "Cancelación de reserva", content);
    }
    public bool SendResetPasswordEmail(string toMailAddress, string content)
    {
        return SendEmail(toMailAddress, "Restablecimiento contraseña", content);
    }
}

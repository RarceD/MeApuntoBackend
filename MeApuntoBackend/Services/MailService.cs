using System.Net;
using System.Net.Mail;

namespace MeApuntoBackend.Services;
public class MailService : IMailService
{
    private MailMessage? _mailMessage;
    private SmtpClient? _smtpClient;
    private string _emailSource;
    private string _host;
    private string _pass;
    public MailService()
    {
        _emailSource = Config.EMAIL_SOURCE;
        _host = Config.HOST;
        _pass = Config.PASS;
    }

    public bool SendEmail(string toMailAddress, string title, string content)
    {
        _smtpClient = new SmtpClient(_host);
        _smtpClient.Credentials = new NetworkCredential(_emailSource, _pass);
        _mailMessage = new MailMessage();
        _mailMessage.From = new MailAddress(_emailSource, _emailSource, System.Text.Encoding.UTF8);
        _mailMessage.To.Add(new MailAddress(toMailAddress));
        _mailMessage.Subject = title;
        _mailMessage.Body = content;
        _mailMessage.IsBodyHtml = true;
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

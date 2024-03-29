﻿using System.Net;
using System.Net.Mail;

namespace MeApuntoBackend.Services;
public class MailService : IMailService
{
    private MailMessage? _mailMessage;
    private SmtpClient? _smtpClient;
    private string _emailSource;
    private string _host;
    private string _pass;

    private readonly ILogger<MailService> _logger;
    public MailService(ILogger<MailService> logger)
    {
        _emailSource = Config.EMAIL_SOURCE;
        _host = Config.HOST;
        _pass = Config.PASS;
        _logger = logger;
    }
    public MailService(ILogger<MailService> logger, string host, string pass, string emailSource)
    {
        _emailSource = emailSource;
        _host = host;
        _pass = pass;
        _logger = logger;
    }

    private bool SendEmail(string toMailAddress, string title, string content)
    {
        _smtpClient = new SmtpClient(_host);
        _smtpClient.Credentials = new NetworkCredential(_emailSource, _pass);
        try
        {
            _mailMessage = new MailMessage();
            _mailMessage.From = new MailAddress(_emailSource, _emailSource, System.Text.Encoding.UTF8);
            _mailMessage.To.Add(new MailAddress(toMailAddress));
            _mailMessage.Subject = title;
            _mailMessage.Body = content;
            _mailMessage.IsBodyHtml = true;
            _smtpClient.Send(_mailMessage);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error sending email to {toMailAddress} - {ex.Message}");
            return false;
        }
    }
    public bool SendConfirmationEmail(string toMailAddress, string day, string hour, string time)
    {
        string content = @"
            <h2> Confirmación de reserva DAY </h2> 
            <h3> Disfrute de su partida a las HOUR durante TIME h </h3>
            <div> Si no pudiese asistir le recomentamos que anule su reserva con tiempo para que cualquier vecino pueda disfrutar de la pista.</div>
            <img src='https://www.meapunto.online/images/logo.png' width = '180' height = '180' >
            <h4> Su aplicación de reservas </h3>
        ";
        content = content.Replace("DAY", day).Replace("TIME", time).Replace("HOUR", hour);
        return SendEmail(toMailAddress, "[MEAPUNTO CONFIRMACIÓN]", content);
    }
    public bool SendCanceledEmail(string toMailAddress, string day, string hour, string time)
    {
        string content = @"
            <h2> Cancelación de reserva DAY </h2> 
            <h3> Ha cancelado su partida a las HOUR durante TIME h </h3>
            <div> Si hubiese cancelado por error le animamos a que vuelva a realizar la reserva, esperemos que ningún vecino se le haya adelantado!</div>
            <img src='https://www.meapunto.online/images/logo.png' width = '180' height = '180' >
            <h4> Su aplicación de reservas </h3>
        ";
        content = content.Replace("DAY", day).Replace("TIME", time).Replace("HOUR", hour);
        return SendEmail(toMailAddress, "[MEAPUNTO CANCELACIÓN]", content);
    }
    public bool SendResetPasswordEmail(string toMailAddress, string newPass)
    {
        string content = @"
            <h2> Restablecimiento de contraseña </h2> 
            <h3> Su nueva contraseña es:<h2> CODE </h2> </h3>
            <div> Si usted no recuerda haber restaurado su contraseña póngase en contacto con meapunto.online@gmail.com </div>
            <img src='https://www.meapunto.online/images/logo.png' width = '180' height = '180' >
            <h4> Su aplicación de reservas </h3>
        ";
        content = content.Replace("CODE", newPass);
        return SendEmail(toMailAddress, "[MEAPUNTO CONTRASEÑA]", content);
    }
}

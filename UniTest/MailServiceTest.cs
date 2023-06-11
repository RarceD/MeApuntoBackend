using MeApuntoBackend.Controllers.Dtos;
using MeApuntoBackend.Services;
using System.Globalization;
using System.Net.Mail;
using System.Runtime.CompilerServices;

namespace UniTest;

public class MailServiceTest
{
    private static string mailAddres = "asdftest1234@gmail.com";
    [Fact]
    public void MakeBookNotValid()
    {
        IMailService bookerManagement = new MailService();
        string hour = "10:00";
        string day = "11/06/2023";
        string time = "1h";
        // bookerManagement.SendCanceledEmail(mailAddres, day, hour, time);
        bookerManagement.SendConfirmationEmail(mailAddres, day, hour, time);
        // bookerManagement.SendResetPasswordEmail(mailAddres, "1234");
        Assert.False(false);
    }
}

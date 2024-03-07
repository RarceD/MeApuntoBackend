using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public static class Utils
{
    public static string GetSha256(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        byte[] hash = SHA256.Create().ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
    public static string GetMD5(string input)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        byte[] hash = MD5.Create().ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
    public static bool IsValidEmail(string email)
    {
        const string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return new Regex(pattern).IsMatch(email);
    }

}

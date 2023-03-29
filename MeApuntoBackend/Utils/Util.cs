using System.Security.Cryptography;
using System.Text;

public static class Utils
{
    public static string Sha256(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        byte[] hash = SHA256.Create().ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}

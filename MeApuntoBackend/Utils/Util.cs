using System.Security.Cryptography;
using System.Text;

public static class Utils
{
    public static string Sha256(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        SHA256 hashstring = SHA256.Create();
        byte[] hash = hashstring.ComputeHash(bytes);
        string hashString = string.Empty;
        foreach (byte x in hash)
            hashString += String.Format("{0:x2}", x);
        return hashString;
    }
}

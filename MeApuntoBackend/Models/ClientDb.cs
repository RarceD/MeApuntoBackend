namespace MeApuntoBackend.Models;
public class ClientDb
{
    public int id { get; set; }
    public int urba_id { get; set; }
    public string? name { get; set; }
    public string? username { get; set; }
    public string? pass { get; set; }
    public string? token { get; set; }
    public int plays { get; set; }
    public string? floor { get; set; }
    public string? letter { get; set; }
    public string? house { get; set; }
    public int role { get; set; }
}

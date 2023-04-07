namespace MeApuntoBackend.Models;
public class BookerDb
{
    public int Id { get; set; }
    public int client_id { get; set; }
    public int court_id { get; set; }
    public int weekday { get; set; }
    public string? time_book { get; set; }
    public string? duration { get; set; }
    public string? Day { get; set; }
}

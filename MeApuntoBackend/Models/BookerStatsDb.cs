namespace MeApuntoBackend.Models;
public class BookerStatsDb
{
    public int Id { get; set; }
    public int CourtId { get; set; }
    public string? BookTime { get; set; }
    public string? RegisterTime { get; set; }
    public bool IsDelete { get; set; }
}

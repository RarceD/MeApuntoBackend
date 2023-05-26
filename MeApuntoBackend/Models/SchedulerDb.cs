namespace MeApuntoBackend.Models;
public class SchedulerDb
{
    public int Id { get; set; }
    public int CourtId { get; set; }
    public int ClientId { get; set; }
    public string? Time { get; set; }
    public string? Day { get; set; }
    public string? Duration { get; set; }
}

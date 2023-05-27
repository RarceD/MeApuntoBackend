namespace MeApuntoBackend.Models;
public class CourtDb
{
    public int Id { get; set; }
    public string? name { get; set; }
    public int urba_id { get; set; }
    public int type { get; set; }
    public string? valid_times { get; set; }
}

public enum CourtType
{
    PADEL = 0,
    TENIS,
    EVENT_ROOMS,
    OTHER
}

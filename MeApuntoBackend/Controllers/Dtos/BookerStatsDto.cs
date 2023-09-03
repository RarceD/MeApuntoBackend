namespace MeApuntoBackend.Controllers.Dtos;
public class BookerStatsDto
{
    public int CourtId { get; set; }
    public DateTime BookTime { get; set; }
    public DateTime RegisterTime { get; set; }
    public bool IsDelete { get; set; }
}


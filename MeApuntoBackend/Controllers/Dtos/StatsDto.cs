namespace MeApuntoBackend.Controllers.Dtos;
public class StatsDto
{
    public int Id { get; set; }
    public string? Token { get; set; }
}
public class StatsResponse
{
    public bool Success { get; set; }
    public string? Date { get; set; }
}

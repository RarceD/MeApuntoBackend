namespace MeApuntoBackend.Controllers.Dtos
{
    public class BookerDto
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public int CourtId { get; set; }
        public string? Time { get; set; }
        public string? Duration { get; set; }
        public string? Day { get; set; }
        public int BookId { get; set; }
    }

    public class BookerResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string? Weekday { get; set; }
        public string? Hour { get; set; }
        public string? CourtName { get; set; }
        public string? Duration { get; set; }
        public string? ClientName { get; set; }
        public int Type { get; set; }
    }
    public enum DurationType
    {
        ONE_HOUR = 1,
        TWO_HOUR,
    }

}

namespace MeApuntoBackend.Controllers.Dtos
{
    public class BookerDto 
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public int CourtId  { get; set; }
        public string? Time  { get; set; }
    }

    public class BookerResponse
    {
        public int CourtId { get; set; }
        public string? CourtName { get; set; }
        public List<BookSchedul>? Scheduler { get; set; }
    }
    public class BookSchedul
    {
        public DateTime Day { get; set; }
        public string? HourAvailable { get; set; }
        public bool Available { get; set; }
        public string? UserName { get; set; }
    }

}

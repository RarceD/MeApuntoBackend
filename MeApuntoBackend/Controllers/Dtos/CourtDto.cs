
namespace MeApuntoBackend.Controllers.Dtos;
public class CourtResponse
{
    public class Timetable
    {
        public int Day { get; set; }
        public string? fullDay { get; set; }
        public List<TimeAvailability>? Availability { get; set; }
    }
    public class TimeAvailability
    {
        public string? Time { get; set; }
        public bool Valid { get; set; }
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public int Type { get; set; }
    public string? ValidTimes { get; set; }
    public List<Timetable>? Timetables { get; set; }
}

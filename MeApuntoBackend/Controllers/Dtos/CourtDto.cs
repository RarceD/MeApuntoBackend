﻿
namespace MeApuntoBackend.Controllers.Dtos;
public class CourtResponse
{
    public class Timetable
    {
        public string? Day { get; set; }
        public string? Time { get; set; }
        public bool Valid { get; set; }
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Type { get; set; }
    public string? ValidTimes { get; set; }
    public List<Timetable>? Timetables { get; set; }
}

﻿namespace MeApuntoBackend.Services.Dto;
public class BookerRecord
{
    public int CourtId { get; set; }
    public string? BookTime { get; set; }
    public string? RegisterTime { get; set; }
    public bool IsDelete { get; set; }
}

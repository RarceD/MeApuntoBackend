﻿namespace MeApuntoBackend.Models;
public class LoginStatsDb
{
    public int Id { get; set; }
    public bool Success { get; set; }
    public bool AutoLogin { get; set; }
    public string? RegisterTime { get; set; }
}

namespace MeApuntoBackend.Controllers.Dtos
{
    public class CourtDto
    {
        public string? User { get; set; }
        public string? Pass { get; set; }
        public string? Key { get; set; }
        public string? Name { get; set; }
        public string? Door { get; set; }
        public string? Floor { get; set; }
        public string? House { get; set; }
    }

    public class CourtResponse
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public bool Success { get; set; }
    }
}

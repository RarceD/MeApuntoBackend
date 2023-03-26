namespace MeApuntoBackend.Controllers.Dtos
{
    public class LoginDto
    {
        public string? User { get; set; }
        public string? Pass { get; set; }
    }

    public class LoginResponse
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public bool Success { get; set; }
    }
}

namespace MeApuntoBackend.Controllers.Dtos
{
    public class CreateDto
    {
        public string? User { get; set; }
        public string? Pass { get; set; }
        public string? Key { get; set; }
        public string? Name { get; set; }
        public string? Door { get; set; }
        public string? Floor { get; set; }
        public string? House { get; set; }
    }

    public class CreateResponse
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public bool Success { get; set; }
    }
    public class RedirectDto
    {
        public string? Code { get; set; }
    }
    public class RedirectResponse
    {
        public bool Success { set; get; } = false;
        public string? Url { get; set; }
    }
}

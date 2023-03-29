namespace MeApuntoBackend.Controllers.Dtos
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public string? Token { get; set; }
    }

    public class ProfileResponse
    {
        public string? Name { get; set; }
        public int plays { get; set; }
        public string? floor { get; set; }
        public string? letter { get; set; }
        public string? urbaName { get; set; }
        public string? username { get; set; }
    }
}

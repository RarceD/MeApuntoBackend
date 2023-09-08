namespace MeApuntoBackend.Controllers.Dtos;
public class PaymentDto
{
    public int Id { get; set; }
    public string? Token { get; set; }
    public string? ProductId { get; set; }
}

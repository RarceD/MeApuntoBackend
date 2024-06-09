using MeApuntoBackend.Services.Enum;

namespace MeApuntoBackend.Services.Dto;
public class WSInteractionDto
{
    public int clientId { get; set; }
    public WSMsgType type { get; set; }
    public object? payload { get; set; }
}

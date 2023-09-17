using MeApuntoBackend.Services.Dto;
using System.Text.Json;

namespace MeApuntoBackend.Services;
public class ExternalDeviceService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUri;

    public ExternalDeviceService()
    {
        _httpClient = new HttpClient();
        _baseUri = string.Empty;
    }
    public void GetDeviceStatus(Action<WSInteractionDto> action, WSInteractionDto received)
    {
        //  Simulate long time response
        Thread.Sleep(1000);
        action(received);
    }

    private async Task<string> GetAsync(string endpoint)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUri}/{endpoint}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                string? result = JsonSerializer.Deserialize<string>(content);
                return result ?? string.Empty;
            }
            else
            {
                throw new HttpRequestException($"API request failed with status code: {response.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}

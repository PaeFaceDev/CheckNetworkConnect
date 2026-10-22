using System.Net.Http.Json;

namespace PingMonitor.Services;

public class FeishuService
{
    private readonly HttpClient _httpClient;

    public FeishuService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendMessageAsync(
        string webhook,
        string message,
        CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            msg_type = "text",
            content = new
            {
                text = message
            }
        };

        var response = await _httpClient.PostAsJsonAsync(
            webhook,
            payload,
            cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(
            cancellationToken);

        Console.WriteLine(
            $"Feishu Response: {(int)response.StatusCode} {response.StatusCode}");

        Console.WriteLine(responseBody);

        response.EnsureSuccessStatusCode();
    }
}
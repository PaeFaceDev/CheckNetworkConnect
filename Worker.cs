using Microsoft.Extensions.Options;
using PingMonitor.Models;
using PingMonitor.Services;

namespace PingMonitor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly PingMonitorOptions _options;
    private readonly PingService _pingService;
    private readonly FeishuService _feishuService;

    public Worker(
        ILogger<Worker> logger,
        IOptions<PingMonitorOptions> options,
        PingService pingService,
        FeishuService feishuService)
    {
        _logger = logger;
        _options = options.Value;
        _pingService = pingService;
        _feishuService = feishuService;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation("Ping Monitor Started");

        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var target in _options.Targets)
            {
                if (stoppingToken.IsCancellationRequested)
                    break;

                var isOnline = await _pingService.PingAsync(
                    target.IpAddress,
                    _options.TimeoutMilliseconds);

                if (isOnline)
                {
                    _logger.LogInformation(
                        "{Name} ({Ip}) - ONLINE",
                        target.Name,
                        target.IpAddress);
                }
                else
                {
                    _logger.LogWarning(
                        "{Name} ({Ip}) - REQUEST TIMEOUT",
                        target.Name,
                        target.IpAddress);

                    var message =
                        $"Request timed out.\n" +
                        $"Server: {target.Name}\n" +
                        $"IP: {target.IpAddress}\n" +
                        $"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";

                    try
                    {
                        await _feishuService.SendMessageAsync(
                            _options.FeishuWebhook,
                            message,
                            stoppingToken);
                    }
                    catch (OperationCanceledException)
                        when (stoppingToken.IsCancellationRequested)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Failed to send Feishu alert for {Name} ({Ip})",
                            target.Name,
                            target.IpAddress);
                    }
                }
            }

            await Task.Delay(
                TimeSpan.FromSeconds(_options.IntervalSeconds),
                stoppingToken);
        }
    }
}

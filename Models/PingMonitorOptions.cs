namespace PingMonitor.Models;

public class PingMonitorOptions
{
    public int IntervalSeconds { get; set; } = 2;

    public int TimeoutMilliseconds { get; set; } = 1000;

    public List<PingTarget> Targets { get; set; } = [];

    public string FeishuWebhook { get; set; } = "";
}
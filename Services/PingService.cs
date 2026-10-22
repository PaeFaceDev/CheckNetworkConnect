using System.Net.NetworkInformation;

namespace PingMonitor.Services;

public class PingService
{
    public async Task<bool> PingAsync(
        string ipAddress,
        int timeoutMilliseconds)
    {
        using var ping = new Ping();

        try
        {
            var reply = await ping.SendPingAsync(
                ipAddress,
                timeoutMilliseconds);

            return reply.Status == IPStatus.Success;
        }
        catch
        {
            return false;
        }
    }
}
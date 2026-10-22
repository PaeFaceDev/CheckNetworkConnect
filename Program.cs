using PingMonitor;
using PingMonitor.Models;
using PingMonitor.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<PingMonitorOptions>(
    builder.Configuration.GetSection("PingMonitor"));

builder.Services.AddSingleton<PingService>();

builder.Services.AddHttpClient<FeishuService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

await host.RunAsync();
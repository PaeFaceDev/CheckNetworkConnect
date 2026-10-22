# PingMonitor

A lightweight background network monitoring application built with C# and .NET Worker Service. It continuously monitors multiple IP addresses and sends real-time notifications to Feishu when a monitored host becomes unreachable.

## Overview

PingMonitor is designed to help developers and system administrators monitor the availability of servers and network devices.

The application runs in the background, periodically sends ICMP ping requests to configured IP addresses, and reports their connectivity status. When a ping request fails, the application sends a notification to a Feishu bot through its webhook API.

## Features

- **Continuous Monitoring** — Periodically checks the availability of configured IP addresses.
- **Multiple IP Support** — Monitors multiple servers and network devices from a single configuration file.
- **Configurable Ping Interval** — Defines how frequently each target is checked.
- **Configurable Timeout** — Sets the maximum time to wait for a ping response.
- **Feishu Notifications** — Sends alerts through the Feishu Bot Webhook API when a ping fails.
- **Structured Logging** — Displays monitoring results and errors through the .NET logging system.
- **Background Execution** — Uses .NET Worker Service to run long-running monitoring tasks.

## Technology Stack

| Technology | Purpose |
|---|---|
| C# | Application development |
| .NET Worker Service | Background task execution |
| ICMP Ping | Network connectivity checks |
| HTTP Client | Sending HTTP requests to Feishu |
| Feishu Bot Webhook API | Alert notifications |
| JSON | Application configuration |

## Project Structure

```text
PingMonitor/
├── Models/
│   ├── PingTarget.cs
│   └── PingMonitorOptions.cs
├── Services/
│   ├── PingService.cs
│   └── FeishuService.cs
├── Program.cs
├── Worker.cs
├── appsettings.json
└── PingMonitor.csproj
```

### Main Components

- **Worker.cs** — Controls the monitoring loop and coordinates ping checks and notifications.
- **PingService.cs** — Sends ICMP ping requests and determines whether a target responds.
- **FeishuService.cs** — Sends alert messages to the configured Feishu webhook.
- **PingTarget.cs** — Defines a monitoring target, including its name and IP address.
- **PingMonitorOptions.cs** — Defines the monitoring configuration.
- **appsettings.json** — Stores monitoring settings and target IP addresses.

## Getting Started

### Prerequisites

- .NET SDK compatible with the project's target framework.
- Network access to the target IP addresses.
- A Feishu bot with a configured webhook URL.

### 1. Clone the Repository

```bash
git clone <your-repository-url>
cd PingMonitor
```

### 2. Configure Monitoring Targets

Edit `appsettings.json`:

```json
{
  "PingMonitor": {
    "IntervalSeconds": 5,
    "TimeoutMilliseconds": 1000,
    "Targets": [
      {
        "Name": "Application Server",
        "IpAddress": "192.168.1.10"
      },
      {
        "Name": "Database Server",
        "IpAddress": "192.168.1.11"
      },
      {
        "Name": "Network Gateway",
        "IpAddress": "192.168.1.1"
      }
    ],
    "FeishuWebhook": "YOUR_FEISHU_WEBHOOK"
  }
}
```

Configuration options:

| Setting | Description | Example |
|---|---|---|
| `IntervalSeconds` | Delay between monitoring cycles | `5` |
| `TimeoutMilliseconds` | Maximum wait time for each ping | `1000` |
| `Targets` | List of hosts to monitor | Server IP addresses |
| `Name` | Human-readable target name | `Application Server` |
| `IpAddress` | IP address to monitor | `192.168.1.10` |
| `FeishuWebhook` | Feishu bot webhook URL | Your webhook URL |

Replace the example IP addresses with the actual addresses you want to monitor.

**Security:** Do not commit a real webhook URL to a public repository. Use environment variables or another secret-management method for production deployments.

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Run the Application

```bash
dotnet run
```

The application starts monitoring the configured targets and writes their statuses to the console.

## Example Output

```text
info: PingMonitor.Worker[0]
      Ping Monitor Started

info: PingMonitor.Worker[0]
      Application Server (192.168.1.10) - ONLINE

info: PingMonitor.Worker[0]
      Database Server (192.168.1.11) - ONLINE

warn: PingMonitor.Worker[0]
      Database Server (192.168.1.11) - REQUEST TIMEOUT
```

When a ping fails, the application sends a notification to Feishu.

Example notification:

```text
Request timed out.
Server: Database Server
IP: 192.168.1.11
Time: 2026-10-22 10:30:00
```

## How It Works

1. Load monitoring settings and target IP addresses from `appsettings.json`.
2. Send an ICMP ping request to each configured target.
3. Determine whether the target responds within the configured timeout.
4. Log the connectivity result.
5. Send a Feishu notification when a ping fails.
6. Wait for the configured interval and repeat the monitoring cycle.

## Current Limitations

- Monitoring targets are configured through `appsettings.json`.
- Notifications are currently sent for every failed ping, so repeated failures may generate duplicate messages.
- The application does not yet persist monitoring history to a database.
- ICMP traffic may be blocked by firewalls or network policies, even when a host or service is otherwise available.
- Running as a Windows Service requires additional deployment configuration.

## Future Improvements

- [ ] Track `Online` and `Down` states for each target.
- [ ] Send notifications only when a target changes from `Online` to `Down`.
- [ ] Send recovery notifications when a target becomes reachable again.
- [ ] Add retry thresholds to reduce false-positive alerts.
- [ ] Write monitoring logs to files or a database.
- [ ] Support dynamic target management without restarting the application.
- [ ] Add a web dashboard for monitoring server status and uptime.
- [ ] Add Windows Service deployment and automatic recovery.
- [ ] Add unit tests and integration tests.

## Purpose

This project demonstrates practical experience with background processing, network monitoring, HTTP API integration, configuration management, dependency injection, and structured logging in the .NET ecosystem.

It also provides a foundation for building a more comprehensive infrastructure monitoring solution.

## License

Choose an appropriate open-source license before distributing this project publicly.
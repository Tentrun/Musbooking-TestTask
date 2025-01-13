using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventBus.RMQ;

internal sealed class EventBusHostedService : IHostedService
{
    private readonly IBusControl _busControl;
    private readonly ILogger<EventBusHostedService> _logger;

    public EventBusHostedService(IBusControl busControl, ILogger<EventBusHostedService> logger)
    {
        _busControl = busControl;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event Bus starting...");
        await _busControl
            .StartAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Event Bus started");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event Bus stopping...");

        await _busControl
            .StopAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Event Bus stopped");
    }
}

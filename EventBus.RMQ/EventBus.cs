using EventBus.RMQ.Interfaces;
using MassTransit;

namespace EventBus.RMQ;

public sealed class EventBus : IEventBus
{
    private readonly IBusControl _busControl;
    private readonly IClientFactory _clientFactory;
    public EventBus(IBusControl busControl)
    {
        _busControl = busControl ?? throw new ArgumentNullException(nameof(busControl));
        _clientFactory = _busControl.CreateClientFactory();
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await _busControl.StartAsync(cancellationToken);
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        return _busControl.Publish(message, cancellationToken);
    }

    public async Task SendAsync<T>(T message, string endpoint, CancellationToken cancellationToken = default)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        var sendEnpoint = await _busControl.GetSendEndpoint(new Uri($"queue:{endpoint}"));
        await sendEnpoint.Send(message, cancellationToken);
    }

    public async Task<TResposne> GetResponseAsync<TRequest, TResposne>(TRequest message, string endpoint, CancellationToken cancellationToken = default)
        where TResposne : class
        where TRequest : class
    {
        var client = _clientFactory.CreateRequestClient<TRequest>(new Uri($"queue:{endpoint}"));
        var result = await client.GetResponse<TResposne>(message, cancellationToken);
        return result.Message;
    }
}
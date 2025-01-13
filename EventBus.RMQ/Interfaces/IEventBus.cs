namespace EventBus.RMQ.Interfaces;

public interface IEventBus
{
    Task<TResposne> GetResponseAsync<TRequest, TResposne>(TRequest message, string endpoint, CancellationToken cancellationToken = default)
        where TRequest : class
        where TResposne : class;
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default);
    Task SendAsync<T>(T message, string endpoint, CancellationToken cancellationToken = default);
    Task StartAsync(CancellationToken cancellationToken = default);
}
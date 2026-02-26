namespace AgroSolutions.MessageBus;

public interface IMessageBus
{
    Task PublishAsync<T>(T message, string queueName, CancellationToken ct = default) where T : class;
    Task SubscribeAsync<T>(string queueName, Func<T, Task> handler, CancellationToken ct = default) where T : class;
}

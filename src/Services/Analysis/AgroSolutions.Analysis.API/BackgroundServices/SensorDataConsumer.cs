using AgroSolutions.Analysis.Application.Services;
using AgroSolutions.Common.Events;
using AgroSolutions.MessageBus;

namespace AgroSolutions.Analysis.API.BackgroundServices;

public class SensorDataConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMessageBus _bus;
    private readonly ILogger<SensorDataConsumer> _logger;

    public SensorDataConsumer(IServiceScopeFactory scopeFactory, IMessageBus bus, ILogger<SensorDataConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _bus = bus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consuming queue sensor-data-queue...");

        await _bus.SubscribeAsync<SensorDataReceivedEvent>("sensor-data-queue", async ev =>
        {
            using var scope = _scopeFactory.CreateScope();
            var engine = scope.ServiceProvider.GetRequiredService<AlertEngine>();
            await engine.ProcessAsync(ev, stoppingToken);
        }, stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}

using AgroSolutions.Analysis.Application.Services;
using AgroSolutions.Common.Events;
using AgroSolutions.MessageBus;
using AgroSolutions.Analysis.API.Telemetry;

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

            var generatedTypes = await engine.ProcessAsync(ev, stoppingToken);

            AnalysisMetrics.ClearAlert(ev.TalhaoId, "DROUGHT_ALERT");
            AnalysisMetrics.ClearAlert(ev.TalhaoId, "PEST_RISK");
            AnalysisMetrics.SetStatus(ev.TalhaoId, "Normal");

            foreach (var type in generatedTypes.Distinct())
                AnalysisMetrics.MarkAlert(ev.TalhaoId, type);

            if (generatedTypes.Contains("DROUGHT_ALERT"))
                AnalysisMetrics.SetStatus(ev.TalhaoId, "Alerta de Seca");
            else if (generatedTypes.Contains("PEST_RISK"))
                AnalysisMetrics.SetStatus(ev.TalhaoId, "Risco de Praga");

        }, stoppingToken);


        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}

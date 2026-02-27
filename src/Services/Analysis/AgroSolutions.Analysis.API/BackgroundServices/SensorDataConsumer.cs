using AgroSolutions.Analysis.Application.Services;
using AgroSolutions.Common.Events;
using AgroSolutions.MessageBus;
using AgroSolutions.Analysis.API.Telemetry;
using Microsoft.Extensions.Caching.Memory;
using AgroSolutions.Analysis.API.Internal;

namespace AgroSolutions.Analysis.API.BackgroundServices;

public class SensorDataConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMessageBus _bus;
    private readonly PropertiesInternalClient _propertiesClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<SensorDataConsumer> _logger;

    public SensorDataConsumer(
        IServiceScopeFactory scopeFactory,
        IMessageBus bus,
        PropertiesInternalClient propertiesClient,
        IMemoryCache cache,
        ILogger<SensorDataConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _bus = bus;
        _propertiesClient = propertiesClient;
        _cache = cache;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consuming queue sensor-data-queue...");

        await _bus.SubscribeAsync<SensorDataReceivedEvent>("sensor-data-queue", async ev =>
        {
            // resolve nome com cache
            var cacheKey = $"talhao-name:{ev.TalhaoId}";
            if (!_cache.TryGetValue(cacheKey, out string? talhaoName))
            {
                talhaoName = await _propertiesClient.GetTalhaoNameAsync(ev.TalhaoId, stoppingToken) ?? "Sem nome";
                _cache.Set(cacheKey, talhaoName, TimeSpan.FromMinutes(10));
            }

            using var scope = _scopeFactory.CreateScope();
            var engine = scope.ServiceProvider.GetRequiredService<AlertEngine>();

            var generatedTypes = await engine.ProcessAsync(ev, stoppingToken);

            // “estado atual” nas métricas
            AnalysisMetrics.ClearAlert(ev.TalhaoId, talhaoName!, "DROUGHT_ALERT");
            AnalysisMetrics.ClearAlert(ev.TalhaoId, talhaoName!, "PEST_RISK");
            AnalysisMetrics.SetStatus(ev.TalhaoId, talhaoName!, "Normal");

            foreach (var type in generatedTypes.Distinct())
                AnalysisMetrics.MarkAlert(ev.TalhaoId, talhaoName!, type);

            if (generatedTypes.Contains("DROUGHT_ALERT"))
                AnalysisMetrics.SetStatus(ev.TalhaoId, talhaoName!, "Alerta de Seca");
            else if (generatedTypes.Contains("PEST_RISK"))
                AnalysisMetrics.SetStatus(ev.TalhaoId, talhaoName!, "Risco de Praga");

        }, stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

}

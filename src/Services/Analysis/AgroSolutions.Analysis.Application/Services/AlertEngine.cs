using AgroSolutions.Analysis.Domain.Entities;
using AgroSolutions.Analysis.Domain.Interfaces;
using AgroSolutions.Common.Events;
using Microsoft.Extensions.Logging;

namespace AgroSolutions.Analysis.Application.Services;

public class AlertEngine
{
    private readonly IEnumerable<IAlertRule> _rules;
    private readonly IAlertRepository _alerts;
    private readonly ILogger<AlertEngine> _logger;

    public AlertEngine(IEnumerable<IAlertRule> rules, IAlertRepository alerts, ILogger<AlertEngine> logger)
    {
        _rules = rules;
        _alerts = alerts;
        _logger = logger;
    }

    public async Task ProcessAsync(SensorDataReceivedEvent sensorData, CancellationToken ct)
    {
        foreach (var rule in _rules)
        {
            var ev = await rule.EvaluateAsync(sensorData, ct);
            if (ev is null) continue;

            _logger.LogWarning("Alert generated {Type} for Talhão {TalhaoId}", ev.AlertType, ev.TalhaoId);

            await _alerts.AddAsync(new Alert
            {
                TalhaoId = ev.TalhaoId,
                AlertType = ev.AlertType,
                Message = ev.Message,
                Severity = ev.Severity,
                GeneratedAt = ev.GeneratedAt
            }, ct);
        }
    }
}

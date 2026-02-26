using AgroSolutions.Analysis.Domain.Interfaces;
using AgroSolutions.Common.Events;
using AgroSolutions.Ingestion.Domain.Interfaces;

namespace AgroSolutions.Analysis.Application.Rules;

public class DroughtAlertRule : IAlertRule
{
    private readonly ISensorReadingRepository _sensorRepo;
    public DroughtAlertRule(ISensorReadingRepository sensorRepo) => _sensorRepo = sensorRepo;

    public async Task<AlertGeneratedEvent?> EvaluateAsync(SensorDataReceivedEvent sensorData, CancellationToken ct = default)
    {
        if (sensorData.SoilMoisture >= 30) return null;

        var since = DateTime.UtcNow.AddHours(-24);
        var readings = await _sensorRepo.GetByTalhaoIdAsync(sensorData.TalhaoId, since, DateTime.UtcNow, ct);
        var list = readings.ToList();

        if (list.Count < 2) return null;
        if (!list.All(r => r.SoilMoisture < 30)) return null;

        return new AlertGeneratedEvent(
            sensorData.TalhaoId,
            "DROUGHT_ALERT",
            $"Alerta de Seca: Umidade do solo abaixo de 30% por mais de 24 horas. Último: {sensorData.SoilMoisture}%",
            "Critical",
            DateTime.UtcNow
        );
    }
}

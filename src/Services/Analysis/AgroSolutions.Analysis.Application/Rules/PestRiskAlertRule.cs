using AgroSolutions.Analysis.Domain.Interfaces;
using AgroSolutions.Common.Events;

namespace AgroSolutions.Analysis.Application.Rules;

public class PestRiskAlertRule : IAlertRule
{
    public Task<AlertGeneratedEvent?> EvaluateAsync(SensorDataReceivedEvent sensorData, CancellationToken ct = default)
    {
        if (sensorData.SoilMoisture > 80 && sensorData.Temperature > 28)
        {
            return Task.FromResult<AlertGeneratedEvent?>(new AlertGeneratedEvent(
                sensorData.TalhaoId,
                "PEST_RISK",
                $"Risco de Praga: Umidade alta ({sensorData.SoilMoisture}%) + Temperatura alta ({sensorData.Temperature}°C).",
                "Warning",
                DateTime.UtcNow
            ));
        }

        return Task.FromResult<AlertGeneratedEvent?>(null);
    }
}

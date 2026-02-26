using AgroSolutions.Common.Events;

namespace AgroSolutions.Analysis.Domain.Interfaces;

public interface IAlertRule
{
    Task<AlertGeneratedEvent?> EvaluateAsync(SensorDataReceivedEvent sensorData, CancellationToken ct = default);
}

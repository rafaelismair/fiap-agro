namespace AgroSolutions.Common.Events;

public record SensorDataReceivedEvent(
    Guid TalhaoId,
    double SoilMoisture,
    double Temperature,
    double Precipitation,
    DateTime Timestamp
);

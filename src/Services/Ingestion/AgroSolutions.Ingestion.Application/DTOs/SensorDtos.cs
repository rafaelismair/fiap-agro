namespace AgroSolutions.Ingestion.Application.DTOs;

public record SensorDataRequest(Guid TalhaoId, double SoilMoisture, double Temperature, double Precipitation);

public record SensorDataResponse(
    string Id,
    Guid TalhaoId,
    double SoilMoisture,
    double Temperature,
    double Precipitation,
    DateTime Timestamp
);

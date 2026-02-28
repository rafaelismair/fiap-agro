namespace AgroSolutions.Ingestion.Application.DTOs;

public class SensorDataRequest
{
    public Guid TalhaoId { get; set; }
    public string? TalhaoName { get; set; }
    public double SoilMoisture { get; set; }
    public double Temperature { get; set; }
    public double Precipitation { get; set; }
}


public record SensorDataResponse(
    string Id,
    Guid TalhaoId,
    double SoilMoisture,
    double Temperature,
    double Precipitation,
    DateTime Timestamp
);

using AgroSolutions.Common;
using AgroSolutions.Ingestion.Application.DTOs;

namespace AgroSolutions.Ingestion.Application.Interfaces;

public interface IIngestionService
{
    Task<Result<SensorDataResponse>> IngestSensorDataAsync(SensorDataRequest request, CancellationToken ct = default);
    Task<Result<IEnumerable<SensorDataResponse>>> GetSensorDataAsync(Guid talhaoId, DateTime? from, DateTime? to, CancellationToken ct = default);
}

using AgroSolutions.Ingestion.Domain.Entities;

namespace AgroSolutions.Ingestion.Domain.Interfaces;

public interface ISensorReadingRepository
{
    Task AddAsync(SensorReading reading, CancellationToken ct = default);
    Task<IEnumerable<SensorReading>> GetByTalhaoIdAsync(Guid talhaoId, DateTime? from, DateTime? to, CancellationToken ct = default);
    Task<IEnumerable<SensorReading>> GetLatestByTalhaoIdAsync(Guid talhaoId, int count = 100, CancellationToken ct = default);
}

using AgroSolutions.Ingestion.Domain.Entities;
using AgroSolutions.Ingestion.Domain.Interfaces;
using MongoDB.Driver;

namespace AgroSolutions.Ingestion.Infrastructure.Repositories;

public class SensorReadingRepository : ISensorReadingRepository
{
    private readonly IMongoCollection<SensorReading> _col;

    public SensorReadingRepository(IMongoDatabase db)
    {
        _col = db.GetCollection<SensorReading>("sensor_readings");

        var indexKeys = Builders<SensorReading>.IndexKeys
            .Ascending(x => x.TalhaoId)
            .Descending(x => x.Timestamp);

        _col.Indexes.CreateOne(new CreateIndexModel<SensorReading>(indexKeys));
    }

    public Task AddAsync(SensorReading reading, CancellationToken ct = default)
        => _col.InsertOneAsync(reading, cancellationToken: ct);

    public async Task<IEnumerable<SensorReading>> GetByTalhaoIdAsync(Guid talhaoId, DateTime? from, DateTime? to, CancellationToken ct = default)
    {
        var fb = Builders<SensorReading>.Filter;
        var filter = fb.Eq(x => x.TalhaoId, talhaoId);
        if (from.HasValue) filter &= fb.Gte(x => x.Timestamp, from.Value);
        if (to.HasValue) filter &= fb.Lte(x => x.Timestamp, to.Value);

        return await _col.Find(filter)
            .SortByDescending(x => x.Timestamp)
            .Limit(1000)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<SensorReading>> GetLatestByTalhaoIdAsync(Guid talhaoId, int count = 100, CancellationToken ct = default)
    {
        return await _col.Find(x => x.TalhaoId == talhaoId)
            .SortByDescending(x => x.Timestamp)
            .Limit(count)
            .ToListAsync(ct);
    }
}

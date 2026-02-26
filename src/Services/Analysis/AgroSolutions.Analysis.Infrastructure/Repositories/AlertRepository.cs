using AgroSolutions.Analysis.Domain.Entities;
using AgroSolutions.Analysis.Domain.Interfaces;
using MongoDB.Driver;

namespace AgroSolutions.Analysis.Infrastructure.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly IMongoCollection<Alert> _col;

    public AlertRepository(IMongoDatabase db)
    {
        _col = db.GetCollection<Alert>("alerts");

        var indexKeys = Builders<Alert>.IndexKeys
            .Ascending(x => x.TalhaoId)
            .Descending(x => x.GeneratedAt);

        _col.Indexes.CreateOne(new CreateIndexModel<Alert>(indexKeys));
    }

    public Task AddAsync(Alert alert, CancellationToken ct = default)
        => _col.InsertOneAsync(alert, cancellationToken: ct);

    public async Task<IEnumerable<Alert>> GetAllByTalhaoIdAsync(Guid talhaoId, CancellationToken ct = default)
        => await _col.Find(x => x.TalhaoId == talhaoId)
            .SortByDescending(x => x.GeneratedAt)
            .Limit(200)
            .ToListAsync(ct);

    public async Task<IEnumerable<Alert>> GetActiveByTalhaoIdAsync(Guid talhaoId, CancellationToken ct = default)
        => await _col.Find(x => x.TalhaoId == talhaoId && x.IsActive)
            .SortByDescending(x => x.GeneratedAt)
            .ToListAsync(ct);
}

using AgroSolutions.Analysis.Domain.Entities;

namespace AgroSolutions.Analysis.Domain.Interfaces;

public interface IAlertRepository
{
    Task AddAsync(Alert alert, CancellationToken ct = default);
    Task<IEnumerable<Alert>> GetAllByTalhaoIdAsync(Guid talhaoId, CancellationToken ct = default);
    Task<IEnumerable<Alert>> GetActiveByTalhaoIdAsync(Guid talhaoId, CancellationToken ct = default);
}

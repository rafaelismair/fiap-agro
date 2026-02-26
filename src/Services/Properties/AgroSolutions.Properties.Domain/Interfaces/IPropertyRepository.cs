using AgroSolutions.Properties.Domain.Entities;

namespace AgroSolutions.Properties.Domain.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Property>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Property property, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

using AgroSolutions.Properties.Domain.Entities;
using AgroSolutions.Properties.Domain.Interfaces;
using AgroSolutions.Properties.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions.Properties.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly PropertiesDbContext _db;
    public PropertyRepository(PropertiesDbContext db) => _db = db;

    public async Task<Property?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Properties.Include(p => p.Talhoes).FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IEnumerable<Property>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _db.Properties.Include(p => p.Talhoes).Where(p => p.UserId == userId).ToListAsync(ct);

    public async Task AddAsync(Property property, CancellationToken ct = default)
        => await _db.Properties.AddAsync(property, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _db.SaveChangesAsync(ct);
}

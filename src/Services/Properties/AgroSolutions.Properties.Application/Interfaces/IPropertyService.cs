using AgroSolutions.Common;
using AgroSolutions.Properties.Application.DTOs;

namespace AgroSolutions.Properties.Application.Interfaces;

public interface IPropertyService
{
    Task<Result<PropertyResponse>> CreatePropertyAsync(Guid userId, CreatePropertyRequest request, CancellationToken ct = default);
    Task<Result<TalhaoResponse>> AddTalhaoAsync(Guid propertyId, AddTalhaoRequest request, CancellationToken ct = default);
    Task<Result<IEnumerable<PropertyResponse>>> GetPropertiesByUserAsync(Guid userId, CancellationToken ct = default);
}

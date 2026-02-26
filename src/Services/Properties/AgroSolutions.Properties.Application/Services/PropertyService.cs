using AgroSolutions.Common;
using AgroSolutions.Properties.Application.DTOs;
using AgroSolutions.Properties.Application.Interfaces;
using AgroSolutions.Properties.Domain.Entities;
using AgroSolutions.Properties.Domain.Interfaces;

namespace AgroSolutions.Properties.Application.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _repo;
    public PropertyService(IPropertyRepository repo) => _repo = repo;

    public async Task<Result<PropertyResponse>> CreatePropertyAsync(Guid userId, CreatePropertyRequest request, CancellationToken ct = default)
    {
        var p = Property.Create(userId, request.Name, request.Location, request.TotalAreaHectares);
        if (!p.IsSuccess) return Result<PropertyResponse>.Failure(p.Error!);

        await _repo.AddAsync(p.Value!, ct);
        await _repo.SaveChangesAsync(ct);

        return Result<PropertyResponse>.Success(Map(p.Value!));
    }

    public async Task<Result<TalhaoResponse>> AddTalhaoAsync(Guid propertyId, AddTalhaoRequest request, CancellationToken ct = default)
    {
        var property = await _repo.GetByIdAsync(propertyId, ct);
        if (property is null) return Result<TalhaoResponse>.Failure("Propriedade não encontrada.");

        var t = property.AddTalhao(request.Name, request.Culture, request.AreaHectares);
        if (!t.IsSuccess) return Result<TalhaoResponse>.Failure(t.Error!);

        await _repo.AddTalhaoAsync(t.Value!, ct);

        await _repo.SaveChangesAsync(ct);

        var talhao = t.Value!;
        return Result<TalhaoResponse>.Success(new TalhaoResponse(talhao.Id, talhao.Name, talhao.Culture, talhao.AreaHectares, talhao.Status));
    }


    public async Task<Result<IEnumerable<PropertyResponse>>> GetPropertiesByUserAsync(Guid userId, CancellationToken ct = default)
    {
        var list = await _repo.GetByUserIdAsync(userId, ct);
        return Result<IEnumerable<PropertyResponse>>.Success(list.Select(Map));
    }

    private static PropertyResponse Map(Property p)
        => new(p.Id, p.Name, p.Location, p.TotalAreaHectares,
            p.Talhoes.Select(t => new TalhaoResponse(t.Id, t.Name, t.Culture, t.AreaHectares, t.Status)));
}

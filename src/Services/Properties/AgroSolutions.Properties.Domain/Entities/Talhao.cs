using AgroSolutions.Common;

namespace AgroSolutions.Properties.Domain.Entities;

public class Talhao : BaseEntity
{
    public Guid PropertyId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Culture { get; private set; } = string.Empty;
    public double AreaHectares { get; private set; }
    public string Status { get; private set; } = "Normal";

    private Talhao() { }

    public static Result<Talhao> Create(Guid propertyId, string name, string culture, double areaHectares)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Talhao>.Failure("Nome do talhão é obrigatório.");
        if (string.IsNullOrWhiteSpace(culture))
            return Result<Talhao>.Failure("Cultura plantada é obrigatória.");
        if (areaHectares <= 0)
            return Result<Talhao>.Failure("Área deve ser maior que zero.");

        return Result<Talhao>.Success(new Talhao
        {
            PropertyId = propertyId,
            Name = name,
            Culture = culture,
            AreaHectares = areaHectares
        });
    }

    public void UpdateStatus(string newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
}

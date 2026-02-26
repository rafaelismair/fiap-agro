using AgroSolutions.Common;

namespace AgroSolutions.Properties.Domain.Entities;

public class Property : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public double TotalAreaHectares { get; private set; }

    private readonly List<Talhao> _talhoes = new();
    public IReadOnlyCollection<Talhao> Talhoes => _talhoes.AsReadOnly();

    private Property() { }

    public static Result<Property> Create(Guid userId, string name, string location, double totalAreaHectares)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Property>.Failure("Nome da propriedade é obrigatório.");
        if (totalAreaHectares <= 0)
            return Result<Property>.Failure("Área total deve ser maior que zero.");

        return Result<Property>.Success(new Property
        {
            UserId = userId,
            Name = name,
            Location = location,
            TotalAreaHectares = totalAreaHectares
        });
    }

    public Result<Talhao> AddTalhao(string name, string culture, double areaHectares)
    {
        var t = Talhao.Create(Id, name, culture, areaHectares);
        if (!t.IsSuccess) return t;
        _talhoes.Add(t.Value!);
        return t;
    }
}

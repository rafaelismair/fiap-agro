namespace AgroSolutions.Properties.Application.DTOs;

public record CreatePropertyRequest(string Name, string Location, double TotalAreaHectares);
public record AddTalhaoRequest(string Name, string Culture, double AreaHectares);

public record PropertyResponse(
    Guid Id, string Name, string Location, double TotalAreaHectares,
    IEnumerable<TalhaoResponse> Talhoes);

public record TalhaoResponse(
    Guid Id, string Name, string Culture, double AreaHectares, string Status);

namespace AgroSolutions.Common.Events;

public record AlertGeneratedEvent(
    Guid TalhaoId,
    string AlertType,
    string Message,
    string Severity,
    DateTime GeneratedAt
);

using AgroSolutions.Common;

namespace AgroSolutions.Identity.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    private User() { }

    public static Result<User> Create(string fullName, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return Result<User>.Failure("Nome completo é obrigatório.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return Result<User>.Failure("E-mail inválido.");
        if (string.IsNullOrWhiteSpace(passwordHash))
            return Result<User>.Failure("Senha é obrigatória.");

        return Result<User>.Success(new User
        {
            FullName = fullName,
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash
        });
    }
}

using AgroSolutions.Common;
using AgroSolutions.Identity.Application.DTOs;
using AgroSolutions.Identity.Application.Interfaces;
using AgroSolutions.Identity.Domain.Entities;
using AgroSolutions.Identity.Domain.Interfaces;

namespace AgroSolutions.Identity.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokens;

    public AuthService(IUserRepository users, ITokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var existing = await _users.GetByEmailAsync(request.Email, ct);
        if (existing is not null)
            return Result<AuthResponse>.Failure("E-mail já cadastrado.");

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var userResult = User.Create(request.FullName, request.Email, hash);
        if (!userResult.IsSuccess)
            return Result<AuthResponse>.Failure(userResult.Error!);

        await _users.AddAsync(userResult.Value!, ct);
        await _users.SaveChangesAsync(ct);

        var token = _tokens.GenerateToken(userResult.Value!);
        return Result<AuthResponse>.Success(new AuthResponse(token, userResult.Value!.FullName, userResult.Value!.Email));
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _users.GetByEmailAsync(request.Email, ct);
        if (user is null) return Result<AuthResponse>.Failure("Credenciais inválidas.");
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<AuthResponse>.Failure("Credenciais inválidas.");

        var token = _tokens.GenerateToken(user);
        return Result<AuthResponse>.Success(new AuthResponse(token, user.FullName, user.Email));
    }
}

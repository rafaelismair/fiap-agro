using AgroSolutions.Common;
using AgroSolutions.Identity.Application.DTOs;

namespace AgroSolutions.Identity.Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
}

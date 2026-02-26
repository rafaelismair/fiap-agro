using AgroSolutions.Identity.Domain.Entities;

namespace AgroSolutions.Identity.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}

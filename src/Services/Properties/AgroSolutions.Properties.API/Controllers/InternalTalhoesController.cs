using AgroSolutions.Properties.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions.Properties.API.Controllers;

[ApiController]
[Route("internal/talhoes")]
public class InternalTalhoesController : ControllerBase
{
    private readonly PropertiesDbContext _db;
    private readonly IConfiguration _config;

    public InternalTalhoesController(PropertiesDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpGet("{talhaoId:guid}")]
    public async Task<IActionResult> GetById(Guid talhaoId, CancellationToken ct)
    {
        var expected = _config["InternalApi:Key"];
        var provided = Request.Headers["X-Internal-Key"].ToString();

        if (string.IsNullOrWhiteSpace(expected) || provided != expected)
            return Unauthorized(new { error = "Invalid internal key" });

        var t = await _db.Talhoes.AsNoTracking()
            .Where(x => x.Id == talhaoId)
            .Select(x => new { id = x.Id, name = x.Name, culture = x.Culture, status = x.Status, propertyId = x.PropertyId })
            .FirstOrDefaultAsync(ct);

        return t is null ? NotFound() : Ok(t);
    }
}

using AgroSolutions.Analysis.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroSolutions.Analysis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlertsController : ControllerBase
{
    private readonly IAlertRepository _repo;
    public AlertsController(IAlertRepository repo) => _repo = repo;

    [HttpGet("talhao/{talhaoId:guid}")]
    public async Task<IActionResult> GetAll(Guid talhaoId, CancellationToken ct)
        => Ok(await _repo.GetAllByTalhaoIdAsync(talhaoId, ct));

    [HttpGet("talhao/{talhaoId:guid}/active")]
    public async Task<IActionResult> GetActive(Guid talhaoId, CancellationToken ct)
        => Ok(await _repo.GetActiveByTalhaoIdAsync(talhaoId, ct));
}

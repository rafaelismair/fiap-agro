using System.Security.Claims;
using AgroSolutions.Properties.Application.DTOs;
using AgroSolutions.Properties.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgroSolutions.Properties.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _service;
    public PropertiesController(IPropertyService service) => _service = service;

    private Guid UserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePropertyRequest request, CancellationToken ct)
    {
        var result = await _service.CreatePropertyAsync(UserId(), request, ct);
        return result.IsSuccess ? Created("", result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpPost("{propertyId:guid}/talhoes")]
    public async Task<IActionResult> AddTalhao(Guid propertyId, [FromBody] AddTalhaoRequest request, CancellationToken ct)
    {
        var result = await _service.AddTalhaoAsync(propertyId, request, ct);
        return result.IsSuccess ? Created("", result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet]
    public async Task<IActionResult> GetMine(CancellationToken ct)
    {
        var result = await _service.GetPropertiesByUserAsync(UserId(), ct);
        return Ok(result.Value);
    }
}

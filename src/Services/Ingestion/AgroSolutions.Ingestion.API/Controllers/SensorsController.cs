using AgroSolutions.Ingestion.Application.DTOs;
using AgroSolutions.Ingestion.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgroSolutions.Ingestion.API.Telemetry;

namespace AgroSolutions.Ingestion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorsController : ControllerBase
{
    private readonly IIngestionService _service;
    public SensorsController(IIngestionService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Ingest([FromBody] SensorDataRequest request, CancellationToken ct)
    {
        IngestionMetrics.Observe(request.TalhaoId, request.SoilMoisture, request.Temperature, request.Precipitation);

        var result = await _service.IngestSensorDataAsync(request, ct);
        return result.IsSuccess ? Created("", result.Value) : BadRequest(new { error = result.Error });
    }


    [HttpGet("{talhaoId:guid}")]
    [Authorize]
    public async Task<IActionResult> Get(Guid talhaoId, [FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken ct)
    {
        var result = await _service.GetSensorDataAsync(talhaoId, from, to, ct);
        return Ok(result.Value);
    }
}

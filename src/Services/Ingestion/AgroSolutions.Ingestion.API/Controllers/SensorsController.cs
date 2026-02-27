using AgroSolutions.Ingestion.Application.DTOs;
using AgroSolutions.Ingestion.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgroSolutions.Ingestion.API.Telemetry;
using AgroSolutions.Ingestion.API.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace AgroSolutions.Ingestion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorsController : ControllerBase
{
    private readonly IIngestionService _service;
    private readonly IMemoryCache _cache;
    private readonly PropertiesInternalClient _properties;

    public SensorsController(IIngestionService service, IMemoryCache cache, PropertiesInternalClient properties)
    {
        _service = service;
        _cache = cache;
        _properties = properties;
    }

    [HttpPost]
    public async Task<IActionResult> Ingest([FromBody] SensorDataRequest request, CancellationToken ct)
    {
        // resolve nome do talhão com cache
        var cacheKey = $"talhao-name:{request.TalhaoId}";
        if (!_cache.TryGetValue(cacheKey, out string? talhaoName))
        {
            talhaoName = await _properties.GetTalhaoNameAsync(request.TalhaoId, ct) ?? "Sem nome";
            _cache.Set(cacheKey, talhaoName, TimeSpan.FromMinutes(10));
        }

        // métricas com label talhaoName (não-nulo)
        IngestionMetrics.Observe(request.TalhaoId, talhaoName!, request.SoilMoisture, request.Temperature, request.Precipitation);

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

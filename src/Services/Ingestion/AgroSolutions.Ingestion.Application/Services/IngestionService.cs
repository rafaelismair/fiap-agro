using AgroSolutions.Common;
using AgroSolutions.Common.Events;
using AgroSolutions.Ingestion.Application.DTOs;
using AgroSolutions.Ingestion.Application.Interfaces;
using AgroSolutions.Ingestion.Domain.Entities;
using AgroSolutions.Ingestion.Domain.Interfaces;
using AgroSolutions.MessageBus;

namespace AgroSolutions.Ingestion.Application.Services;

public class IngestionService : IIngestionService
{
    private readonly ISensorReadingRepository _repo;
    private readonly IMessageBus _bus;

    public IngestionService(ISensorReadingRepository repo, IMessageBus bus)
    {
        _repo = repo;
        _bus = bus;
    }

    public async Task<Result<SensorDataResponse>> IngestSensorDataAsync(SensorDataRequest request, CancellationToken ct = default)
    {
        if (request.SoilMoisture < 0 || request.SoilMoisture > 100)
            return Result<SensorDataResponse>.Failure("Umidade do solo deve estar entre 0 e 100%.");
        if (request.Temperature < -50 || request.Temperature > 60)
            return Result<SensorDataResponse>.Failure("Temperatura fora do intervalo aceitável.");

        var reading = new SensorReading
        {
            TalhaoId = request.TalhaoId,
            SoilMoisture = request.SoilMoisture,
            Temperature = request.Temperature,
            Precipitation = request.Precipitation,
            Timestamp = DateTime.UtcNow
        };

        await _repo.AddAsync(reading, ct);

        await _bus.PublishAsync(
            new SensorDataReceivedEvent(reading.TalhaoId, reading.SoilMoisture, reading.Temperature, reading.Precipitation, reading.Timestamp),
            "sensor-data-queue",
            ct);

        return Result<SensorDataResponse>.Success(new SensorDataResponse(
            reading.Id!, reading.TalhaoId, reading.SoilMoisture, reading.Temperature, reading.Precipitation, reading.Timestamp));
    }

    public async Task<Result<IEnumerable<SensorDataResponse>>> GetSensorDataAsync(Guid talhaoId, DateTime? from, DateTime? to, CancellationToken ct = default)
    {
        var readings = await _repo.GetByTalhaoIdAsync(talhaoId, from, to, ct);
        return Result<IEnumerable<SensorDataResponse>>.Success(
            readings.Select(r => new SensorDataResponse(r.Id!, r.TalhaoId, r.SoilMoisture, r.Temperature, r.Precipitation, r.Timestamp)));
    }
}

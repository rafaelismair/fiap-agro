using Prometheus;

namespace AgroSolutions.Ingestion.API.Telemetry;
public static class IngestionMetrics
{
    public static readonly Gauge SoilMoisturePercent = Metrics.CreateGauge(
        "agro_sensor_soil_moisture_percent",
        "Última umidade do solo (%) recebida por talhão.",
        new GaugeConfiguration { LabelNames = new[] { "talhaoId" } });

    public static readonly Gauge TemperatureCelsius = Metrics.CreateGauge(
        "agro_sensor_temperature_celsius",
        "Última temperatura (°C) recebida por talhão.",
        new GaugeConfiguration { LabelNames = new[] { "talhaoId" } });

    public static readonly Gauge PrecipitationMm = Metrics.CreateGauge(
        "agro_sensor_precipitation_mm",
        "Última precipitação (mm) recebida por talhão.",
        new GaugeConfiguration { LabelNames = new[] { "talhaoId" } });

    public static readonly Counter ReadingsTotal = Metrics.CreateCounter(
        "agro_sensor_readings_total",
        "Total de leituras recebidas por talhão.",
        new CounterConfiguration { LabelNames = new[] { "talhaoId" } });

    public static void Observe(Guid talhaoId, double soilMoisture, double temperature, double precipitation)
    {
        var id = talhaoId.ToString();

        SoilMoisturePercent.WithLabels(id).Set(soilMoisture);
        TemperatureCelsius.WithLabels(id).Set(temperature);
        PrecipitationMm.WithLabels(id).Set(precipitation);

        ReadingsTotal.WithLabels(id).Inc();
    }
}

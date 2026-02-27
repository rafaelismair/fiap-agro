using Prometheus;

namespace AgroSolutions.Ingestion.API.Telemetry;

public static class IngestionMetrics
{
    // Labels: talhaoId + talhaoName
    private static readonly string[] Labels = new[] { "talhaoId", "talhaoName" };

    public static readonly Gauge SoilMoisturePercent = Metrics.CreateGauge(
        "agro_sensor_soil_moisture_percent",
        "Última umidade do solo (%) recebida por talhão.",
        new GaugeConfiguration { LabelNames = Labels });

    public static readonly Gauge TemperatureCelsius = Metrics.CreateGauge(
        "agro_sensor_temperature_celsius",
        "Última temperatura (°C) recebida por talhão.",
        new GaugeConfiguration { LabelNames = Labels });

    public static readonly Gauge PrecipitationMm = Metrics.CreateGauge(
        "agro_sensor_precipitation_mm",
        "Última precipitação (mm) recebida por talhão.",
        new GaugeConfiguration { LabelNames = Labels });

    public static void Observe(Guid talhaoId, string talhaoName, double soilMoisture, double temperature, double precipitation)
    {
        var id = talhaoId.ToString();
        var name = string.IsNullOrWhiteSpace(talhaoName) ? "Sem nome" : talhaoName.Trim();

        SoilMoisturePercent.WithLabels(id, name).Set(soilMoisture);
        TemperatureCelsius.WithLabels(id, name).Set(temperature);
        PrecipitationMm.WithLabels(id, name).Set(precipitation);
    }
}

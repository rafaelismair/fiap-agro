using Prometheus;

namespace AgroSolutions.Analysis.API.Telemetry;

public static class AnalysisMetrics
{
    private static readonly string[] AlertLabels = new[] { "talhaoId", "talhaoName", "type" };
    private static readonly string[] StatusLabels = new[] { "talhaoId", "talhaoName", "status" };

    public static readonly Counter AlertsGeneratedTotal = Metrics.CreateCounter(
        "agro_alerts_generated_total",
        "Total de alertas gerados por talhão e tipo.",
        new CounterConfiguration { LabelNames = AlertLabels });

    public static readonly Gauge AlertActive = Metrics.CreateGauge(
        "agro_alert_active",
        "Alerta ativo (0/1) por talhão e tipo (estado atual).",
        new GaugeConfiguration { LabelNames = AlertLabels });

    public static readonly Gauge TalhaoStatus = Metrics.CreateGauge(
        "agro_talhao_status",
        "Status geral do talhão (0/1).",
        new GaugeConfiguration { LabelNames = StatusLabels });

    public static void SetStatus(Guid talhaoId, string talhaoName, string status)
    {
        var id = talhaoId.ToString();
        var name = NormalizeName(talhaoName);

        // zera conhecidos
        TalhaoStatus.WithLabels(id, name, "Normal").Set(0);
        TalhaoStatus.WithLabels(id, name, "Alerta de Seca").Set(0);
        TalhaoStatus.WithLabels(id, name, "Risco de Praga").Set(0);

        TalhaoStatus.WithLabels(id, name, status).Set(1);
    }

    public static void MarkAlert(Guid talhaoId, string talhaoName, string type)
    {
        var id = talhaoId.ToString();
        var name = NormalizeName(talhaoName);

        AlertsGeneratedTotal.WithLabels(id, name, type).Inc();
        AlertActive.WithLabels(id, name, type).Set(1);
    }

    public static void ClearAlert(Guid talhaoId, string talhaoName, string type)
    {
        var id = talhaoId.ToString();
        var name = NormalizeName(talhaoName);

        AlertActive.WithLabels(id, name, type).Set(0);
    }

    private static string NormalizeName(string? talhaoName)
        => string.IsNullOrWhiteSpace(talhaoName) ? "Sem nome" : talhaoName.Trim();
}

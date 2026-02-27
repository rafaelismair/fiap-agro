using Prometheus;

namespace AgroSolutions.Analysis.API.Telemetry;

public static class AnalysisMetrics
{
    public static readonly Counter AlertsGeneratedTotal = Metrics.CreateCounter(
        "agro_alerts_generated_total",
        "Total de alertas gerados por talhão e tipo.",
        new CounterConfiguration { LabelNames = new[] { "talhaoId", "type" } });

    public static readonly Gauge AlertActive = Metrics.CreateGauge(
        "agro_alert_active",
        "Alerta ativo (0/1) por talhão e tipo (estado atual).",
        new GaugeConfiguration { LabelNames = new[] { "talhaoId", "type" } });

    public static readonly Gauge TalhaoStatus = Metrics.CreateGauge(
        "agro_talhao_status",
        "Status geral do talhão (0/1).",
        new GaugeConfiguration { LabelNames = new[] { "talhaoId", "status" } });

    public static void SetStatus(Guid talhaoId, string status)
    {
        var id = talhaoId.ToString();

        TalhaoStatus.WithLabels(id, "Normal").Set(0);
        TalhaoStatus.WithLabels(id, "Alerta de Seca").Set(0);
        TalhaoStatus.WithLabels(id, "Risco de Praga").Set(0);

        TalhaoStatus.WithLabels(id, status).Set(1);
    }

    public static void MarkAlert(Guid talhaoId, string type)
    {
        var id = talhaoId.ToString();
        AlertsGeneratedTotal.WithLabels(id, type).Inc();
        AlertActive.WithLabels(id, type).Set(1);
    }

    public static void ClearAlert(Guid talhaoId, string type)
    {
        var id = talhaoId.ToString();
        AlertActive.WithLabels(id, type).Set(0);
    }
}

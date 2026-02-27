using FluentAssertions;
using AgroSolutions.Analysis.Application.Rules;
using AgroSolutions.Common.Events;
using Xunit;

namespace AgroSolutions.Analysis.UnitTests;

public class PestRiskAlertRuleTests
{
    [Fact(Skip = "Ajustar para refletir a regra real de PestRiskAlertRule")]
    public async Task Should_generate_pest_risk_when_temperature_and_humidity_indicate_risk()
    {
        var rule = new PestRiskAlertRule();

        var ev = new SensorDataReceivedEvent(
            TalhaoId: Guid.NewGuid(),
            SoilMoisture: 70,
            Temperature: 34,
            Precipitation: 10,
            Timestamp: DateTime.UtcNow);

        var alertType = await rule.EvaluateAsync(ev, CancellationToken.None);

        alertType.Should().Be("PEST_RISK");
    }
}

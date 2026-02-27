using FluentAssertions;
using Moq;
using Xunit;
using AgroSolutions.Analysis.Application.Rules;
using AgroSolutions.Common.Events;
using AgroSolutions.Ingestion.Domain.Interfaces;

namespace AgroSolutions.Analysis.UnitTests.Rules;

public class DroughtAlertRuleTests
{
    [Fact]
    public async Task Should_not_generate_drought_alert_when_soil_moisture_is_ok()
    {
        // arrange
        var sensorRepo = new Mock<ISensorReadingRepository>();
        var rule = new DroughtAlertRule(sensorRepo.Object);

        var ev = new SensorDataReceivedEvent(
            TalhaoId: Guid.NewGuid(),
            SoilMoisture: 60,
            Temperature: 25,
            Precipitation: 0,
            Timestamp: DateTime.UtcNow);

        // act
        var alertType = await rule.EvaluateAsync(ev, CancellationToken.None);

        // assert
        alertType.Should().BeNull();
    }

    [Fact(Skip = "Ajustar mock do ISensorReadingRepository para simular janela de 24h")]
    public async Task Should_generate_drought_alert_when_soil_moisture_is_low()
    {
        // arrange
        var sensorRepo = new Mock<ISensorReadingRepository>();
        var rule = new DroughtAlertRule(sensorRepo.Object);

        var ev = new SensorDataReceivedEvent(
            TalhaoId: Guid.NewGuid(),
            SoilMoisture: 10,
            Temperature: 35,
            Precipitation: 0,
            Timestamp: DateTime.UtcNow);

        // act
        var alertType = await rule.EvaluateAsync(ev, CancellationToken.None);

        // assert
        alertType.Should().Be("DROUGHT_ALERT");
    }
}

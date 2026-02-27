using FluentAssertions;
using Xunit;

namespace AgroSolutions.Properties.UnitTests;

public class PropertyServiceTests
{
    [Fact]
    public void Placeholder_Should_pass()
    {
        true.Should().BeTrue();
    }
}

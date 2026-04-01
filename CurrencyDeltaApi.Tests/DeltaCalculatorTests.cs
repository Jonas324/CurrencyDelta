using CurrencyDeltaApi.Services;

namespace CurrencyDeltaApi.Tests;

public class DeltaCalculatorTests
{
    [Fact]
    public void Calculate_ReturnsRateDifferenceRoundedToThreeDecimals()
    {
        var fromRate = 1.20000m;
        var toRate   = 1.18944m;

        var delta = DeltaCalculator.Calculate(fromRate, toRate);

        Assert.Equal(-0.011m, delta);
    }
}

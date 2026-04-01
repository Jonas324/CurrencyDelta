namespace CurrencyDeltaApi.Services;

public static class DeltaCalculator
{
    public static decimal Calculate(decimal fromRate, decimal toRate)
    {
        return Math.Round(toRate - fromRate, 3);
    }
}

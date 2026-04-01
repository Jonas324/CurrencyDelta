using CurrencyDeltaApi.Models;
using CurrencyDeltaApi.Services;
using Moq;

namespace CurrencyDeltaApi.Tests;

public class CurrencyDeltaServiceTests
{
    [Fact]
    public async Task CalculateAsync_ReturnsCorrectDeltaForSingleCurrency()
    {
        // ARRANGE
        var mockRiksbank = new Mock<IRiksbankService>();

        mockRiksbank
            .Setup(s => s.GetRatesAsync("GBP", "USD", "2025-01-01", "2025-01-10"))
            .ReturnsAsync(new List<RiksbankObservation>
            {
                new() { Date = "2025-01-02", Value = 1.20000m },
                new() { Date = "2025-01-09", Value = 1.18944m }
            });

        var service = new CurrencyDeltaService(mockRiksbank.Object);

        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = ["USD"],
            FromDate = "2025-01-01",
            ToDate = "2025-01-10"
        };

        // ACT
        var results = await service.CalculateAsync(request);

        // ASSERT
        Assert.Single(results);
        Assert.Equal("USD", results[0].Currency);
        Assert.Equal(-0.011m, results[0].Delta);
    }

    [Fact]
    public async Task CalculateAsync_ThrowsWhenCurrencyNotFound()
    {
        // ARRANGE
        var mockRiksbank = new Mock<IRiksbankService>();

        mockRiksbank
            .Setup(s => s.GetRatesAsync("GBP", "BANANA", "2025-01-01", "2025-01-10"))
            .ThrowsAsync(new HttpRequestException("Response status code does not indicate success: 404 (Not Found)."));

        var service = new CurrencyDeltaService(mockRiksbank.Object);

        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = ["BANANA"],
            FromDate = "2025-01-01",
            ToDate = "2025-01-10"
        };

        // ACT & ASSERT
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CalculateAsync(request));
        Assert.Contains("BANANA", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}

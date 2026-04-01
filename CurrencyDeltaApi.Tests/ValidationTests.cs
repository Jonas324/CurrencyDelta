using CurrencyDeltaApi.Models;
using CurrencyDeltaApi.Validation;

namespace CurrencyDeltaApi.Tests;

public class ValidationTests
{
    private readonly RequestValidator _validator = new();

    [Fact]
    public void Validate_DuplicateCurrencies_ReturnsError()
    {
        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = ["USD", "USD"],
            FromDate = "2025-01-01",
            ToDate = "2025-01-10"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Equal("duplicatecurrency", result.ErrorCode);
    }

    [Fact]
    public void Validate_CurrencySameAsBaseline_ReturnsError()
    {
        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = ["GBP", "USD"],
            FromDate = "2025-01-01",
            ToDate = "2025-01-10"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Equal("duplicatecurrency", result.ErrorCode);
    }

    [Fact]
    public void Validate_ToDatNotGreaterThanFromDate_ReturnsError()
    {
        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = ["USD"],
            FromDate = "2025-01-10",
            ToDate = "2025-01-01"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Equal("dateproblem", result.ErrorCode);
    }

    [Fact]
    public void Validate_EqualDates_ReturnsError()
    {
        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = ["USD"],
            FromDate = "2025-01-01",
            ToDate = "2025-01-01"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Equal("dateproblem", result.ErrorCode);
    }

    [Fact]
    public void Validate_FromDateBefore2023_ReturnsError()
    {
        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = ["USD"],
            FromDate = "2022-12-31",
            ToDate = "2023-01-10"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Equal("dateproblem", result.ErrorCode);
    }

    [Fact]
    public void Validate_InvalidDateFormat_ReturnsError()
    {
        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = ["USD"],
            FromDate = "01-01-2025",
            ToDate = "10-01-2025"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Equal("dateproblem", result.ErrorCode);
    }

    [Fact]
    public void Validate_ValidRequest_ReturnsSuccess()
    {
        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = ["USD", "SEK"],
            FromDate = "2025-01-01",
            ToDate = "2025-01-10"
        };

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }
}
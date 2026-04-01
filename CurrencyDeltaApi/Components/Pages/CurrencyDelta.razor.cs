using CurrencyDeltaApi.Models;
using CurrencyDeltaApi.Services;
using CurrencyDeltaApi.Validation;
using Microsoft.AspNetCore.Components;

namespace CurrencyDeltaApi.Components.Pages;

public partial class CurrencyDelta
{
    [Inject] private CurrencyDeltaService CurrencyDeltaService { get; set; } = default!;
    [Inject] private RequestValidator Validator { get; set; } = default!;

    private string baseline = "GBP";
    private string compareCurrency = "USD";
    private DateTime fromDate = DateTime.Today.AddMonths(-1);
    private DateTime toDate = DateTime.Today;
    private List<CurrencyDeltaResult>? results;
    private string? errorMessage;

    private async Task Calculate()
    {
        errorMessage = null;
        results = null;

        var request = new CurrencyDeltaRequest
        {
            Baseline = baseline,
            Currencies = [compareCurrency],
            FromDate = fromDate.ToString("yyyy-MM-dd"),
            ToDate = toDate.ToString("yyyy-MM-dd")
        };

        var validation = Validator.Validate(request);
        if (!validation.IsValid)
        {
            errorMessage = validation.ErrorDetails;
            return;
        }

        try
        {
            results = await CurrencyDeltaService.CalculateAsync(request);
        }
        catch (Exception ex)
        {
            errorMessage = $"Request failed: {ex.Message}";
        }
    }
}

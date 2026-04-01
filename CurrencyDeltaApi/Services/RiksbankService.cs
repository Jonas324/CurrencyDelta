using System.Net.Http.Json;
using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Services;

public class RiksbankService(HttpClient httpClient) : IRiksbankService
{
    private const string BaseUrl = "https://api.riksbank.se/swea/v1";

    public async Task<List<RiksbankObservation>> GetRatesAsync(string baseline, string currency, string fromDate, string toDate)
    {
        bool baselineIsSek = baseline.Equals("SEK", StringComparison.OrdinalIgnoreCase);
        bool currencyIsSek = currency.Equals("SEK", StringComparison.OrdinalIgnoreCase);

        string url;

        if (baselineIsSek)
        {
            // e.g. /observations/sekusdpmi/2025-01-01/2025-01-10
            string series = $"sek{currency.ToLower()}pmi";
            url = $"{BaseUrl}/observations/{series}/{fromDate}/{toDate}";
        }
        else
        {
            // e.g. /CrossRates/sekgbppmi/sekusdpmi/2025-01-01/2025-01-10
            string baselineSeries = $"sek{baseline.ToLower()}pmi";
            string currencySeries = currencyIsSek ? $"sek{baseline.ToLower()}pmi" : $"sek{currency.ToLower()}pmi";
            url = $"{BaseUrl}/CrossRates/{baselineSeries}/{currencySeries}/{fromDate}/{toDate}";
        }

        var observations = await httpClient.GetFromJsonAsync<List<RiksbankObservation>>(url)
            ?? [];

        if (currencyIsSek && !baselineIsSek)
        {
            // Invert: Riksbanken has no direct "X per SEK" series, so we invert
            foreach (var obs in observations)
                obs.Value = obs.Value == 0 ? 0 : 1 / obs.Value;
        }

        return observations;
    }
}

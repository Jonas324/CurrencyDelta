using System.Text.Json;
using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Services;

public class CurrencyDeltaService(IRiksbankService riksbank)
{
    public async Task<List<CurrencyDeltaResult>> CalculateAsync(CurrencyDeltaRequest request)
    {
        var results = new List<CurrencyDeltaResult>();

        foreach (var currency in request.Currencies)
        {
            List<RiksbankObservation> observations;
            try
            {
                observations = await riksbank.GetRatesAsync(request.Baseline, currency, request.FromDate, request.ToDate);
            }
            catch (Exception ex) when (ex is HttpRequestException or JsonException)
            {
                throw new InvalidOperationException($"Unsupported or invalid currency: {currency}", ex);
            }

            if (observations.Count == 0)
                throw new InvalidOperationException($"Unsupported or invalid currency: {currency}");

            var fromRate = observations.First().Value;
            var toRate = observations.Last().Value;
            var delta = DeltaCalculator.Calculate(fromRate, toRate);

            results.Add(new CurrencyDeltaResult { Currency = currency, Delta = delta });
        }

        return results;
    }
}

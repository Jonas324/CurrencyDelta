using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Services;

public interface IRiksbankService
{
    Task<List<RiksbankObservation>> GetRatesAsync(string baseline, string currency, string fromDate, string toDate) ;
}

namespace CurrencyDeltaApi.Models;

public class CurrencyDeltaRequest
{
    public string Baseline { get; set; } = string.Empty;
    public List<string> Currencies { get; set; } = [];
    public string FromDate { get; set; } = string.Empty;
    public string ToDate { get; set; } = string.Empty;
}

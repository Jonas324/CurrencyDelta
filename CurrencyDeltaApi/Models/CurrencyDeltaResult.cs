namespace CurrencyDeltaApi.Models;

public class CurrencyDeltaResult
{
    public string Currency { get; set; } = string.Empty;
    public decimal Delta { get; set; }
}

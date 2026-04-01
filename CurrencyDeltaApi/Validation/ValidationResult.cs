namespace CurrencyDeltaApi.Validation;

public class ValidationResult
{
    public bool IsValid { get; init; }
    public string? ErrorCode { get; init; }
    public string? ErrorDetails { get; init; }

    public static ValidationResult Success() => new() { IsValid = true };

    public static ValidationResult Failure(string errorCode, string errorDetails) =>
        new() { IsValid = false, ErrorCode = errorCode, ErrorDetails = errorDetails };
}

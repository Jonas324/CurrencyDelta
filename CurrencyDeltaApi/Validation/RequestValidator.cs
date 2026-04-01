using CurrencyDeltaApi.Models;

namespace CurrencyDeltaApi.Validation;

public class RequestValidator
{
    private const string DateFormat = "yyyy-MM-dd";

    public ValidationResult Validate(CurrencyDeltaRequest request)
    {
        if (request.Currencies.Count != request.Currencies.Distinct(StringComparer.OrdinalIgnoreCase).Count())
            return ValidationResult.Failure("duplicatecurrency", "The currencies list contains duplicates");

        if (request.Currencies.Any(c => c.Equals(request.Baseline, StringComparison.OrdinalIgnoreCase)))
            return ValidationResult.Failure("duplicatecurrency", "A currency must not be the same as the baseline");

        if (!DateOnly.TryParseExact(request.FromDate, DateFormat, out var fromDate))
            return ValidationResult.Failure("dateproblem", "fromDate is not in a valid yyyy-MM-dd format");

        if (!DateOnly.TryParseExact(request.ToDate, DateFormat, out var toDate))
            return ValidationResult.Failure("dateproblem", "toDate is not in a valid yyyy-MM-dd format");

        if (fromDate.Year < 2023)
            return ValidationResult.Failure("dateproblem", "fromDate must not be earlier than 2023");

        if (toDate <= fromDate)
            return ValidationResult.Failure("dateproblem", "To date is smaller than or equal to from date");

        return ValidationResult.Success();
    }
}

using CurrencyDeltaApi.Models;
using CurrencyDeltaApi.Services;
using CurrencyDeltaApi.Validation;

namespace CurrencyDeltaApi.Endpoints;

public static class CurrencyDeltaEndpoints
{
    public static void MapCurrencyDeltaEndpoints(this WebApplication app)
    {
        app.MapPost("/currencydelta", async (CurrencyDeltaRequest request, RequestValidator validator, CurrencyDeltaService service) =>
        {
            var validation = validator.Validate(request);
            if (!validation.IsValid)
                return Results.BadRequest(new ErrorResponse { ErrorCode = validation.ErrorCode ?? string.Empty, ErrorDetails = validation.ErrorDetails ?? string.Empty });

            try
            {
                var results = await service.CalculateAsync(request);
                return Results.Ok(results);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new ErrorResponse { ErrorCode = "invalidcurrency", ErrorDetails = ex.Message });
            }
        });
    }
}

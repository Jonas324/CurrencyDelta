# CurrencyDelta API — Todo List

## Project Setup
- [x] Create a new `dotnet new webapi` project named `CurrencyDeltaApi`
- [x] Create a `CurrencyDeltaApi.Tests` xUnit test project
- [x] Add both to the solution file

## Models
- [x] Create `CurrencyDeltaRequest` model (baseline, currencies, fromDate, toDate)
- [x] Create `CurrencyDeltaResult` model (currency, delta)
- [x] Create `ErrorResponse` model (errorCode, errorDetails)

## Validation
- [x] Validate date format (`yyyy-MM-dd`)
- [x] Validate `fromDate` is not before 2023
- [x] Validate `toDate > fromDate`
- [x] Validate no duplicate currencies
- [x] Validate no currency matches the baseline
- [x] Validate all currencies are real/supported (after fetching from Riksbanken)

## Riksbanken Integration
- [x] Create `IRiksbankService` interface
- [x] Implement `RiksbankService` with `HttpClient` (registered via DI)
- [x] Handle SEK baseline → use `/observations` endpoint
- [x] Handle non-SEK baseline → use `/CrossRates` endpoint
- [x] Handle SEK as a target currency (invert the rate)
- [x] Handle non-bank days (pick nearest available observation)

## Endpoint
- [x] Create `POST /currencydelta` endpoint
- [x] Wire up validation → return `400` with error body on failure
- [x] Wire up service → calculate delta and return `200` on success

## Unit Tests
- [x] Test: duplicate currencies returns correct error
- [x] Test: `toDate <= fromDate` returns correct error
- [x] Test: `fromDate` before 2023 returns correct error
- [x] Test: currency same as baseline returns correct error
- [x] Test: invalid/unsupported currency returns correct error
- [x] Test: valid request returns correct delta calculation (mock Riksbanken service)

## Finish
- [x] Verify README matches final project structure and ports
- [x] Push to a git repository

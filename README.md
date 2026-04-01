# CurrencyDelta API

A REST API that calculates the exchange rate delta between a baseline currency and a list of target currencies over a given date range, powered by Riksbanken's public API.

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Internet access (to reach the Riksbanken API)

---

## How to Run Locally

1. Clone the repository:
   ```bash
   git clone <repo-url>
   cd CurrencyDelta
   ```

2. Navigate to the API project:
   ```bash
   cd CurrencyDeltaApi
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. The API will be available at:
   ```
   https://localhost:7191
   http://localhost:5263
   ```

---

## Endpoint

### `POST /currencydelta`

Calculates the exchange rate delta for each target currency relative to the baseline currency between two dates.

#### Request Body

```json
{
  "baseline": "GBP",
  "currencies": ["USD", "SEK"],
  "fromDate": "2025-01-01",
  "toDate": "2025-01-10"
}
```

| Field        | Type             | Description                                         |
|--------------|------------------|-----------------------------------------------------|
| `baseline`   | `string`         | The base currency code (e.g. `"GBP"`, `"SEK"`)     |
| `currencies` | `array<string>`  | List of target currencies to compare against        |
| `fromDate`   | `string`         | Start date in `yyyy-MM-dd` format                   |
| `toDate`     | `string`         | End date in `yyyy-MM-dd` format                     |

#### Success Response — `200 OK`

```json
[
  {
    "currency": "USD",
    "delta": -0.011
  }
]
```

The `delta` is the difference in exchange rate (toDate rate minus fromDate rate), rounded to 3 decimal places, expressed relative to the baseline currency.

#### Error Response — `400 Bad Request`

```json
{
  "errorCode": "dateproblem",
  "errorDetails": "To date is smaller than or equal to from date"
}
```

---

## Validation Rules

| Error Code          | Condition                                                         |
|---------------------|-------------------------------------------------------------------|
| `duplicatecurrency` | The `currencies` list contains duplicates                         |
| `dateproblem`       | Dates are invalid, equal, or `toDate` is not after `fromDate`     |
| `dateproblem`       | `fromDate` is earlier than 2023-01-01                             |
| `invalidcurrency`   | One or more currencies do not exist / are not supported           |
| `duplicatecurrency` | One or more currencies in the list are the same as the baseline   |

---

## How Delta is Calculated

The API uses [Riksbanken's SWEA API](https://api.riksbank.se/swea/v1) to fetch exchange rate observations.

- **Baseline is SEK**: Uses the `/observations/{series}/{from}/{to}` endpoint directly.
- **Baseline is not SEK**: Uses the `/CrossRates/{series1}/{series2}/{from}/{to}` endpoint, where cross rates are derived relative to SEK.
- **SEK as a target currency**: Rate is inverted (`1 / observed value`).
- **Non-bank days**: If a date falls on a weekend or holiday, the nearest available observation is used.

Exchange series are constructed as: `sek` + `{lowercase currency}` + `pmi`  
Example: USD → `sekusdpmi`, GBP → `sekgbppmi`

---

## Running Unit Tests

```bash
cd CurrencyDeltaApi.Tests
dotnet test
```

---

## Project Structure

```
CurrencyDelta/
├── CurrencyDeltaApi/               # Main API project
│   ├── Endpoints/
│   ├── Models/
│   ├── Services/
│   ├── Validation/
│   └── Program.cs
├── CurrencyDeltaApi.Tests/         # Unit test project
└── README.md
```

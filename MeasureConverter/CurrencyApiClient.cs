using System.Net.Http.Json;

namespace MeasureConverter;

public class CurrencyApiClient : ICurrencyRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public CurrencyApiClient(
        HttpClient httpClient,
        string apiKey)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException(
                "API key is required.",
                nameof(apiKey));
        }

        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<double> GetRateAsync(
        string baseCurrency,
        string destinationCurrency)
    {
        string url =
            $"https://api.currencyapi.com/v3/latest" +
            $"?base_currency={baseCurrency}" +
            $"&currencies={destinationCurrency}";

        using HttpRequestMessage request =
            new HttpRequestMessage(HttpMethod.Get, url);

        request.Headers.Add("apikey", _apiKey);

        using HttpResponseMessage response =
            await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        CurrencyApiResponse? result =
            await response.Content.ReadFromJsonAsync<CurrencyApiResponse>();

        if (result?.Data == null ||
            !result.Data.TryGetValue(destinationCurrency, out CurrencyRate? rate))
        {
            throw new InvalidOperationException(
                "Currency rate was not returned by the API.");
        }

        return rate.Value;
    }
}

public class CurrencyApiResponse
{
    public Dictionary<string, CurrencyRate> Data { get; set; } = new();
}

public class CurrencyRate
{
    public string Code { get; set; } = string.Empty;

    public double Value { get; set; }
}
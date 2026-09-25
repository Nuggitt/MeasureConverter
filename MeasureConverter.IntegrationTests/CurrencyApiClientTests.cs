using MeasureConverter;

namespace MeasureConverter.IntegrationTests;

public class CurrencyApiClientTests
{
    [Theory]
    [InlineData("DKK", "EUR")]
    [InlineData("EUR", "DKK")]
    [InlineData("DKK", "USD")]
    public async Task GetRateAsync_ValidCurrencies_ReturnsPositiveRate(
        string baseCurrency,
        string destinationCurrency)
    {
        // Arrange
        string? apiKey =
            Environment.GetEnvironmentVariable("CURRENCY_API_KEY");

        Assert.False(
            string.IsNullOrWhiteSpace(apiKey),
            "CURRENCY_API_KEY environment variable is not set.");

        HttpClient httpClient = new();

        CurrencyApiClient client =
            new CurrencyApiClient(httpClient, apiKey!);

        // Act
        double rate =
            await client.GetRateAsync(
                baseCurrency,
                destinationCurrency);

        // Assert
        Assert.True(rate > 0);
    }
    
    [Fact]
    public async Task GetRateAsync_InvalidCurrency_ThrowsHttpRequestException()
    {
        // Arrange
        string? apiKey =
            Environment.GetEnvironmentVariable("CURRENCY_API_KEY");

        Assert.False(
            string.IsNullOrWhiteSpace(apiKey),
            "CURRENCY_API_KEY environment variable is not set.");

        HttpClient httpClient = new();

        CurrencyApiClient client =
            new CurrencyApiClient(httpClient, apiKey!);

        // Act
        Func<Task> act = () =>
            client.GetRateAsync("DKK", "INVALID");

        // Assert
        await Assert.ThrowsAsync<HttpRequestException>(act);
    }
}
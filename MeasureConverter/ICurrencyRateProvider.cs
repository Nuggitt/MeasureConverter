namespace MeasureConverter;

public interface ICurrencyRateProvider
{
    Task<double> GetRateAsync(
        string baseCurrency,
        string destinationCurrency);
}
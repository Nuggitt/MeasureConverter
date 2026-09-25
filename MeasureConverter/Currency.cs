namespace MeasureConverter;

public class Currency
{
    private readonly string _baseCurrency;
    private readonly ICurrencyRateProvider _rateProvider;

    public Currency(
        string baseCurrency,
        ICurrencyRateProvider rateProvider)
    {
        if (string.IsNullOrWhiteSpace(baseCurrency))
        {
            throw new ArgumentException(
                "Base currency is required.",
                nameof(baseCurrency));
        }

        ArgumentNullException.ThrowIfNull(rateProvider);

        _baseCurrency = baseCurrency;
        _rateProvider = rateProvider;
    }

    public async Task<double> ConvertAsync(
        double amount,
        string destinationCurrency)
    {
        if (double.IsNaN(amount) ||
            double.IsInfinity(amount) ||
            amount < 0)
        {
            throw new ArgumentException(
                "Amount must be a non-negative finite number.",
                nameof(amount));
        }

        if (string.IsNullOrWhiteSpace(destinationCurrency))
        {
            throw new ArgumentException(
                "Destination currency is required.",
                nameof(destinationCurrency));
        }

        double rate = await _rateProvider.GetRateAsync(
            _baseCurrency,
            destinationCurrency);

        return Math.Round(amount * rate, 2);
    }
}
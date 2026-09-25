namespace MeasureConverter;

public class Currency
{
    private readonly string _baseCurrency;
    private readonly ICurrencyRateProvider _rateProvider;

    public Currency(
        string baseCurrency,
        ICurrencyRateProvider rateProvider)
    {
        ValidateCurrencyCode(
            baseCurrency,
            nameof(baseCurrency));

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

        ValidateCurrencyCode(
            destinationCurrency,
            nameof(destinationCurrency));

        double rate = await _rateProvider.GetRateAsync(
            _baseCurrency,
            destinationCurrency);

        return Math.Round(amount * rate, 2);
    }

    private static void ValidateCurrencyCode(
        string currencyCode,
        string parameterName)
    {
        if (string.IsNullOrWhiteSpace(currencyCode) ||
            currencyCode.Length != 3 ||
            !currencyCode.All(char.IsLetter))
        {
            throw new ArgumentException(
                "Currency code must contain exactly three letters.",
                parameterName);
        }
    }
}
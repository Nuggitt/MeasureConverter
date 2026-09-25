using MeasureConverter;
using Moq;

namespace MeasureConverter.Tests;

public class CurrencyTests
{
    // Equivalence Partitioning + Paradigmatic Values
    [Fact]
    public async Task ConvertAsync_ValidAmountAndCurrencies_ReturnsExpectedValue()
    {
        // Arrange
        Mock<ICurrencyRateProvider> rateProviderMock = new();

        rateProviderMock
            .Setup(provider => provider.GetRateAsync("DKK", "EUR"))
            .ReturnsAsync(0.13);

        Currency currency =
            new Currency("DKK", rateProviderMock.Object);

        // Act
        double actual =
            await currency.ConvertAsync(100.00, "EUR");

        // Assert
        Assert.Equal(13.00, actual);
    }

    // Boundary Value Analysis - At and Above Boundary
    [Theory]
    [InlineData(0.00, 0.00)]
    [InlineData(0.01, 0.01)]
    public async Task ConvertAsync_AmountAtOrAboveBoundary_ReturnsExpectedValue(
        double amount,
        double expected)
    {
        // Arrange
        Mock<ICurrencyRateProvider> rateProviderMock = new();

        rateProviderMock
            .Setup(provider => provider.GetRateAsync("DKK", "EUR"))
            .ReturnsAsync(1.00);

        Currency currency =
            new Currency("DKK", rateProviderMock.Object);

        // Act
        double actual =
            await currency.ConvertAsync(amount, "EUR");

        // Assert
        Assert.Equal(expected, actual);
    }

    // Boundary Value Analysis - Below Boundary
    [Fact]
    public async Task ConvertAsync_AmountBelowBoundary_ThrowsArgumentException()
    {
        // Arrange
        Mock<ICurrencyRateProvider> rateProviderMock = new();

        Currency currency =
            new Currency("DKK", rateProviderMock.Object);

        double amount = -0.01;

        // Act
        Func<Task> act = () =>
            currency.ConvertAsync(amount, "EUR");

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    // Extreme / Special Values
    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public async Task ConvertAsync_NonFiniteAmount_ThrowsArgumentException(
        double amount)
    {
        // Arrange
        Mock<ICurrencyRateProvider> rateProviderMock = new();

        Currency currency =
            new Currency("DKK", rateProviderMock.Object);

        // Act
        Func<Task> act = () =>
            currency.ConvertAsync(amount, "EUR");

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    // Invalid Equivalence Partition - Base Currency
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidBaseCurrency_ThrowsArgumentException(
        string? baseCurrency)
    {
        // Arrange
        Mock<ICurrencyRateProvider> rateProviderMock = new();

        // Act
        Action act = () =>
        {
            _ = new Currency(
                baseCurrency!,
                rateProviderMock.Object);
        };

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // Invalid Equivalence Partition - Destination Currency
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ConvertAsync_InvalidDestinationCurrency_ThrowsArgumentException(
        string? destinationCurrency)
    {
        // Arrange
        Mock<ICurrencyRateProvider> rateProviderMock = new();

        Currency currency =
            new Currency("DKK", rateProviderMock.Object);

        // Act
        Func<Task> act = () =>
            currency.ConvertAsync(
                100.00,
                destinationCurrency!);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    // Invalid Dependency
    [Fact]
    public void Constructor_NullRateProvider_ThrowsArgumentNullException()
    {
        // Arrange
        ICurrencyRateProvider? rateProvider = null;

        // Act
        Action act = () =>
        {
            _ = new Currency(
                "DKK",
                rateProvider!);
        };

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    // Extreme Value
    [Fact]
    public async Task ConvertAsync_MaxValueWithNeutralRate_ReturnsFiniteValue()
    {
        // Arrange
        Mock<ICurrencyRateProvider> rateProviderMock = new();

        rateProviderMock
            .Setup(provider => provider.GetRateAsync("DKK", "EUR"))
            .ReturnsAsync(1.00);

        Currency currency =
            new Currency("DKK", rateProviderMock.Object);

        // Act
        double actual =
            await currency.ConvertAsync(
                double.MaxValue,
                "EUR");

        // Assert
        Assert.True(double.IsFinite(actual));
    }

    // Dependency Interaction
    [Fact]
    public async Task ConvertAsync_ValidCurrencies_CallsRateProviderWithCorrectCurrencies()
    {
        // Arrange
        Mock<ICurrencyRateProvider> rateProviderMock = new();

        rateProviderMock
            .Setup(provider => provider.GetRateAsync("DKK", "EUR"))
            .ReturnsAsync(0.13);

        Currency currency =
            new Currency("DKK", rateProviderMock.Object);

        // Act
        await currency.ConvertAsync(100.00, "EUR");

        // Assert
        rateProviderMock.Verify(
            provider => provider.GetRateAsync("DKK", "EUR"),
            Times.Once);
    }
}
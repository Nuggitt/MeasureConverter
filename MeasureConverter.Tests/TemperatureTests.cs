using MeasureConverter;

namespace MeasureConverter.Tests;

public class TemperatureTests
{
    // Equivalence Partitioning + Paradigmatic Values
    [Theory]
    [InlineData(0.00, 32.00)]
    [InlineData(100.00, 212.00)]
    [InlineData(-40.00, -40.00)]
    public void Convert_CelsiusToFahrenheit_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Temperature temperature =
            new Temperature(measure, TemperatureScale.Celsius);

        // Act
        double actual =
            temperature.Convert(TemperatureScale.Fahrenheit);

        // Assert
        Assert.Equal(expected, actual);
    }

    // Equivalence Partitioning + Paradigmatic Values
    [Theory]
    [InlineData(0.00, 273.15)]
    [InlineData(100.00, 373.15)]
    public void Convert_CelsiusToKelvin_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Temperature temperature =
            new Temperature(measure, TemperatureScale.Celsius);

        // Act
        double actual =
            temperature.Convert(TemperatureScale.Kelvin);

        // Assert
        Assert.Equal(expected, actual);
    }

    // Equivalence Partitioning + Paradigmatic Values
    [Theory]
    [InlineData(32.00, 0.00)]
    [InlineData(212.00, 100.00)]
    [InlineData(-40.00, -40.00)]
    public void Convert_FahrenheitToCelsius_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Temperature temperature =
            new Temperature(measure, TemperatureScale.Fahrenheit);

        // Act
        double actual =
            temperature.Convert(TemperatureScale.Celsius);

        // Assert
        Assert.Equal(expected, actual);
    }

    // Equivalence Partitioning + Paradigmatic Values
    [Theory]
    [InlineData(32.00, 273.15)]
    [InlineData(212.00, 373.15)]
    public void Convert_FahrenheitToKelvin_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Temperature temperature =
            new Temperature(measure, TemperatureScale.Fahrenheit);

        // Act
        double actual =
            temperature.Convert(TemperatureScale.Kelvin);

        // Assert
        Assert.Equal(expected, actual);
    }

    // Equivalence Partitioning + Paradigmatic Values
    [Theory]
    [InlineData(273.15, 0.00)]
    [InlineData(373.15, 100.00)]
    public void Convert_KelvinToCelsius_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Temperature temperature =
            new Temperature(measure, TemperatureScale.Kelvin);

        // Act
        double actual =
            temperature.Convert(TemperatureScale.Celsius);

        // Assert
        Assert.Equal(expected, actual);
    }

    // Equivalence Partitioning + Paradigmatic Values
    [Theory]
    [InlineData(273.15, 32.00)]
    [InlineData(373.15, 212.00)]
    public void Convert_KelvinToFahrenheit_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Temperature temperature =
            new Temperature(measure, TemperatureScale.Kelvin);

        // Act
        double actual =
            temperature.Convert(TemperatureScale.Fahrenheit);

        // Assert
        Assert.Equal(expected, actual);
    }

    // Invalid Equivalence Partition - Same Source and Destination
    [Theory]
    [InlineData(TemperatureScale.Celsius)]
    [InlineData(TemperatureScale.Fahrenheit)]
    [InlineData(TemperatureScale.Kelvin)]
    public void Convert_SameSourceAndDestinationScale_ThrowsArgumentException(
        TemperatureScale scale)
    {
        // Arrange
        Temperature temperature = new Temperature(100.00, scale);

        // Act
        Action act = () => temperature.Convert(scale);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // Boundary Value Analysis - Below Absolute Zero
    [Theory]
    [InlineData(-273.16, TemperatureScale.Celsius)]
    [InlineData(-459.68, TemperatureScale.Fahrenheit)]
    [InlineData(-0.01, TemperatureScale.Kelvin)]
    public void Constructor_TemperatureBelowAbsoluteZero_ThrowsArgumentException(
        double measure,
        TemperatureScale scale)
    {
        // Act
        Action act = () =>
        {
            _ = new Temperature(
                measure,
                scale);
        };

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // Boundary Value Analysis - Celsius At and Above Boundary
    [Theory]
    [InlineData(-273.15, -459.67)]
    [InlineData(-273.14, -459.65)]
    public void Convert_CelsiusBoundaryValuesToFahrenheit_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Temperature temperature =
            new Temperature(measure, TemperatureScale.Celsius);

        // Act
        double actual =
            temperature.Convert(TemperatureScale.Fahrenheit);

        // Assert
        Assert.Equal(expected, actual);
    }

    // Boundary Value Analysis - Fahrenheit At and Above Boundary
    [Theory]
    [InlineData(-459.67, -273.15)]
    [InlineData(-459.66, -273.14)]
    public void Convert_FahrenheitBoundaryValuesToCelsius_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Temperature temperature =
            new Temperature(measure, TemperatureScale.Fahrenheit);

        // Act
        double actual =
            temperature.Convert(TemperatureScale.Celsius);

        // Assert
        Assert.Equal(expected, actual);
    }

    // Boundary Value Analysis - Kelvin At and Above Boundary
    [Theory]
    [InlineData(0.00, -273.15)]
    [InlineData(0.01, -273.14)]
    public void Convert_KelvinBoundaryValuesToCelsius_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Temperature temperature =
            new Temperature(measure, TemperatureScale.Kelvin);

        // Act
        double actual =
            temperature.Convert(TemperatureScale.Celsius);

        // Assert
        Assert.Equal(expected, actual);
    }

    // Extreme / Special Values
    [Theory]
    [InlineData(double.NaN, TemperatureScale.Celsius)]
    [InlineData(double.NaN, TemperatureScale.Fahrenheit)]
    [InlineData(double.NaN, TemperatureScale.Kelvin)]
    [InlineData(double.PositiveInfinity, TemperatureScale.Celsius)]
    [InlineData(double.PositiveInfinity, TemperatureScale.Fahrenheit)]
    [InlineData(double.PositiveInfinity, TemperatureScale.Kelvin)]
    [InlineData(double.NegativeInfinity, TemperatureScale.Celsius)]
    [InlineData(double.NegativeInfinity, TemperatureScale.Fahrenheit)]
    [InlineData(double.NegativeInfinity, TemperatureScale.Kelvin)]
    public void Constructor_NonFiniteTemperature_ThrowsArgumentException(
        double measure,
        TemperatureScale scale)
    {
        // Act
        Action act = () =>
        {
            _ = new Temperature(
                measure,
                scale);
        };

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
    
    // Extreme Values
    [Theory]
    [InlineData(TemperatureScale.Celsius, TemperatureScale.Kelvin)]
    [InlineData(TemperatureScale.Kelvin, TemperatureScale.Celsius)]
    public void Convert_MaxValueWithoutOverflow_ReturnsFiniteValue(
        TemperatureScale sourceScale,
        TemperatureScale destinationScale)
    {
        // Arrange
        Temperature temperature =
            new Temperature(double.MaxValue, sourceScale);

        // Act
        double actual = temperature.Convert(destinationScale);

        // Assert
        Assert.True(double.IsFinite(actual));
    }
}
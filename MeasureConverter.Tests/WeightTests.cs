using MeasureConverter;

namespace MeasureConverter.Tests;

public class WeightTests
{
    // Equivalence Partitioning + Paradigmatic Values
    [Theory]
    [InlineData(1.00, 2.20)]
    [InlineData(10.00, 22.05)]
    [InlineData(100.00, 220.46)]
    public void Convert_MetricToImperial_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Weight weight = new Weight(measure, MeasurementSystem.Metric);

        // Act
        double actual = weight.Convert();

        // Assert
        Assert.Equal(expected, actual);
    }

    // Equivalence Partitioning + Paradigmatic Values
    [Theory]
    [InlineData(1.00, 0.45)]
    [InlineData(10.00, 4.54)]
    [InlineData(100.00, 45.36)]
    public void Convert_ImperialToMetric_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Weight weight = new Weight(measure, MeasurementSystem.Imperial);

        // Act
        double actual = weight.Convert();

        // Assert
        Assert.Equal(expected, actual);
    }

    // Boundary Value Analysis - Below Boundary
    [Theory]
    [InlineData(-0.01, MeasurementSystem.Metric)]
    [InlineData(-0.01, MeasurementSystem.Imperial)]
    public void Constructor_MeasureBelowBoundary_ThrowsArgumentException(
        double measure,
        MeasurementSystem system)
    {
        // Act
        Action act = () =>
        {
            _ = new Weight(
                measure,
                system);
        };

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // Boundary Value Analysis - At and Above Boundary
    [Theory]
    [InlineData(0.00, 0.00)]
    [InlineData(0.01, 0.02)]
    public void Convert_MetricBoundaryValues_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Weight weight = new Weight(measure, MeasurementSystem.Metric);

        // Act
        double actual = weight.Convert();

        // Assert
        Assert.Equal(expected, actual);
    }
    
    // Boundary Value Analysis - At and Above Boundary
    [Theory]
    [InlineData(0.00, 0.00)]
    [InlineData(0.01, 0.00)]
    public void Convert_ImperialBoundaryValues_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Weight weight = new Weight(measure, MeasurementSystem.Imperial);

        // Act
        double actual = weight.Convert();

        // Assert
        Assert.Equal(expected, actual);
    }

    // Extreme / Special Values
    [Theory]
    [InlineData(double.NaN, MeasurementSystem.Metric)]
    [InlineData(double.NaN, MeasurementSystem.Imperial)]
    [InlineData(double.PositiveInfinity, MeasurementSystem.Metric)]
    [InlineData(double.PositiveInfinity, MeasurementSystem.Imperial)]
    [InlineData(double.NegativeInfinity, MeasurementSystem.Metric)]
    [InlineData(double.NegativeInfinity, MeasurementSystem.Imperial)]
    public void Constructor_NonFiniteMeasure_ThrowsArgumentException(
        double measure,
        MeasurementSystem system)
    {
        // Act
        Action act = () =>
        {
            _ = new Weight(
                measure,
                system);
        };

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // Extreme Value
    [Fact]
    public void Convert_MaxValueImperial_ReturnsFiniteValue()
    {
        // Arrange
        Weight weight =
            new Weight(double.MaxValue, MeasurementSystem.Imperial);

        // Act
        double actual = weight.Convert();

        // Assert
        Assert.True(double.IsFinite(actual));
    }
    
    
}
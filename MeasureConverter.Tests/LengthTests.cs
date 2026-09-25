using MeasureConverter;

namespace MeasureConverter.Tests;

public class LengthTests
{
    [Theory]
    [InlineData(0.00, 0.00)]
    [InlineData(1.00, 0.39)]
    [InlineData(2.54, 1.00)]
    [InlineData(100.00, 39.37)]
    public void Convert_MetricToImperial_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Length length = new Length(measure, MeasurementSystem.Metric);

        // Act
        double actual = length.Convert();

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(0.00, 0.00)]
    [InlineData(1.00, 2.54)]
    [InlineData(10.00, 25.40)]
    [InlineData(100.00, 254.00)]
    public void Convert_ImperialToMetric_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Length length = new Length(measure, MeasurementSystem.Imperial);

        // Act
        double actual = length.Convert();

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(-0.01)]
    public void Constructor_MeasureBelowBoundary_ThrowsArgumentException(double measure)
    {
        // Act
        Action act = () => new Length(measure, MeasurementSystem.Metric);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
    
    [Theory]
    [InlineData(0.00, 0.00)]
    [InlineData(0.01, 0.00)]
    public void Convert_MetricBoundaryValues_ReturnsExpectedValue(
        double measure,
        double expected)
    {
        // Arrange
        Length length = new Length(measure, MeasurementSystem.Metric);

        // Act
        double actual = length.Convert();

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Constructor_NonFiniteMeasure_ThrowsArgumentException(double measure)
    {
        // Act
        Action act = () => new Length(measure, MeasurementSystem.Metric);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }
    
    [Fact]
    public void Convert_MaxValueMetric_ReturnsFiniteValue()
    {
        // Arrange
        Length length =
            new Length(double.MaxValue, MeasurementSystem.Metric);

        // Act
        double actual = length.Convert();

        // Assert
        Assert.True(double.IsFinite(actual));
    }
}
namespace MeasureConverter;

public class Temperature
{
    private readonly double _measure;
    private readonly TemperatureScale _scale;

    public Temperature(double measure, TemperatureScale scale)
    {
        if (double.IsNaN(measure) || double.IsInfinity(measure))
        {
            throw new ArgumentException(
                "Temperature must be a finite number.",
                nameof(measure));
        }

        if (scale == TemperatureScale.Celsius && measure < -273.15)
        {
            throw new ArgumentException(
                "Celsius temperature cannot be below absolute zero.",
                nameof(measure));
        }

        if (scale == TemperatureScale.Fahrenheit && measure < -459.67)
        {
            throw new ArgumentException(
                "Fahrenheit temperature cannot be below absolute zero.",
                nameof(measure));
        }

        if (scale == TemperatureScale.Kelvin && measure < 0)
        {
            throw new ArgumentException(
                "Kelvin temperature cannot be below absolute zero.",
                nameof(measure));
        }

        _measure = measure;
        _scale = scale;
    }
    
    public double Convert(TemperatureScale destinationScale)
    {
        switch (_scale, destinationScale)
        {
            case (TemperatureScale.Celsius, TemperatureScale.Fahrenheit):
                return CelsiusToFahrenheit();

            case (TemperatureScale.Celsius, TemperatureScale.Kelvin):
                return CelsiusToKelvin();

            case (TemperatureScale.Fahrenheit, TemperatureScale.Celsius):
                return FahrenheitToCelsius();

            case (TemperatureScale.Fahrenheit, TemperatureScale.Kelvin):
                return FahrenheitToKelvin();

            case (TemperatureScale.Kelvin, TemperatureScale.Celsius):
                return KelvinToCelsius();

            case (TemperatureScale.Kelvin, TemperatureScale.Fahrenheit):
                return KelvinToFahrenheit();

            default:
                throw new ArgumentException("Source and destination scales must be different.");
        }
    }
    
    private double CelsiusToFahrenheit()
    {
        return Math.Round((_measure * 9 / 5) + 32, 2);
    }

    private double CelsiusToKelvin()
    {
        return Math.Round(_measure + 273.15, 2);
    }

    private double FahrenheitToCelsius()
    {
        return Math.Round((_measure - 32) * 5 / 9, 2);
    }

    private double FahrenheitToKelvin()
    {
        return Math.Round((_measure - 32) * 5 / 9 + 273.15, 2);
    }

    private double KelvinToCelsius()
    {
        return Math.Round(_measure - 273.15, 2);
    }

    private double KelvinToFahrenheit()
    {
        return Math.Round((_measure - 273.15) * 9 / 5 + 32, 2);
    }
}
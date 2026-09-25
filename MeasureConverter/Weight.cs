namespace MeasureConverter;

public class Weight
{
    private readonly double _measure;
    private readonly MeasurementSystem _system;

    public Weight(double measure, MeasurementSystem system)
    {
        if (double.IsNaN(measure) ||
            double.IsInfinity(measure) ||
            measure < 0)
        {
            throw new ArgumentException(
                "Measure must be a non-negative finite number.",
                nameof(measure));
        }

        _measure = measure;
        _system = system;
    }

    public double Convert()
    {
        if (_system == MeasurementSystem.Metric)
        {
            return Math.Round(_measure * 2.20462, 2); // Metric -> Imperial
        }
        else
        {
            return Math.Round(_measure / 2.20462, 2); // Imperial -> Metric
        }
    }
}
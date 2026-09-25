namespace MeasureConverter;

public class Grade
{
    private readonly IGradeRepository _gradeRepository;

    public Grade(IGradeRepository gradeRepository)
    {
        ArgumentNullException.ThrowIfNull(gradeRepository);

        _gradeRepository = gradeRepository;
    }

    public async Task<string> ConvertAsync(
        string grade,
        GradeSystem sourceSystem)
    {
        if (string.IsNullOrWhiteSpace(grade))
        {
            throw new ArgumentException(
                "Grade is required.",
                nameof(grade));
        }

        string? convertedGrade =
            await _gradeRepository.GetConvertedGradeAsync(
                grade,
                sourceSystem);

        if (convertedGrade is null)
        {
            throw new ArgumentException(
                "The grade could not be converted.",
                nameof(grade));
        }

        return convertedGrade;
    }
}
namespace MeasureConverter;

public interface IGradeRepository
{
    Task<string?> GetConvertedGradeAsync(
        string grade,
        GradeSystem sourceSystem);
}
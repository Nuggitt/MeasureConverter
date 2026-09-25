using MeasureConverter;

namespace MeasureConverter.IntegrationTests;

public class GradeDatabaseIntegrationTests
{
    [Theory]
    [InlineData("12", GradeSystem.Danish, "A")]
    [InlineData("A", GradeSystem.American, "12")]
    public async Task ConvertAsync_ValidGradeAgainstDatabase_ReturnsExpectedGrade(
        string grade,
        GradeSystem sourceSystem,
        string expected)
    {
        // Arrange
        string databasePath =
            Path.Combine(
                Path.GetTempPath(),
                $"grades-{Guid.NewGuid()}.db");

        string connectionString =
            $"Data Source={databasePath};Pooling=False";

        await GradeDatabaseInitializer.InitializeAsync(
            connectionString);

        SqliteGradeRepository repository =
            new SqliteGradeRepository(connectionString);

        Grade converter =
            new Grade(repository);

        // Act
        string actual =
            await converter.ConvertAsync(
                grade,
                sourceSystem);

        // Assert
        Assert.Equal(expected, actual);

        File.Delete(databasePath);
    }
}
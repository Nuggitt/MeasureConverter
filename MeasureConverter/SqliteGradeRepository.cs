using Microsoft.Data.Sqlite;

namespace MeasureConverter;

public class SqliteGradeRepository : IGradeRepository
{
    private readonly string _connectionString;

    public SqliteGradeRepository(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(
                "Connection string is required.",
                nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    public async Task<string?> GetConvertedGradeAsync(
        string grade,
        GradeSystem sourceSystem)
    {
        await using SqliteConnection connection =
            new SqliteConnection(_connectionString);

        await connection.OpenAsync();

        await using SqliteCommand command =
            connection.CreateCommand();

        command.CommandText =
            """
            SELECT TargetGrade
            FROM GradeConversions
            WHERE SourceSystem = $sourceSystem
              AND SourceGrade = $sourceGrade
            LIMIT 1;
            """;

        command.Parameters.AddWithValue(
            "$sourceSystem",
            sourceSystem.ToString());

        command.Parameters.AddWithValue(
            "$sourceGrade",
            grade);

        object? result =
            await command.ExecuteScalarAsync();

        return result as string;
    }
}
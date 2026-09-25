using Microsoft.Data.Sqlite;

namespace MeasureConverter;

public static class GradeDatabaseInitializer
{
    public static async Task InitializeAsync(
        string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(
                "Connection string is required.",
                nameof(connectionString));
        }

        await using SqliteConnection connection =
            new SqliteConnection(connectionString);

        await connection.OpenAsync();

        await using SqliteCommand command =
            connection.CreateCommand();

        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS GradeConversions
            (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SourceSystem TEXT NOT NULL,
                SourceGrade TEXT NOT NULL,
                TargetGrade TEXT NOT NULL,
                UNIQUE(SourceSystem, SourceGrade)
            );

            INSERT OR IGNORE INTO GradeConversions
                (SourceSystem, SourceGrade, TargetGrade)
            VALUES
                ('Danish', '12', 'A'),
                ('Danish', '10', 'B'),
                ('Danish', '7', 'C'),
                ('Danish', '4', 'D'),
                ('Danish', '02', 'D'),
                ('Danish', '00', 'F'),
                ('Danish', '-3', 'F'),

                ('American', 'A', '12'),
                ('American', 'B', '10'),
                ('American', 'C', '7'),
                ('American', 'D', '02'),
                ('American', 'F', '00');
            """;

        await command.ExecuteNonQueryAsync();
    }
}
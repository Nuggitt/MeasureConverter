using MeasureConverter;
using Moq;

namespace MeasureConverter.Tests;

public class GradeTests
{
    // Equivalence Partitioning + Paradigmatic Values
    [Theory]
    [InlineData("12", GradeSystem.Danish, "A")]
    [InlineData("7", GradeSystem.Danish, "C")]
    [InlineData("A", GradeSystem.American, "12")]
    [InlineData("C", GradeSystem.American, "7")]
    public async Task ConvertAsync_ValidGrade_ReturnsExpectedGrade(
        string grade,
        GradeSystem sourceSystem,
        string expected)
    {
        // Arrange
        Mock<IGradeRepository> repositoryMock = new();

        repositoryMock
            .Setup(repository =>
                repository.GetConvertedGradeAsync(
                    grade,
                    sourceSystem))
            .ReturnsAsync(expected);

        Grade converter =
            new Grade(repositoryMock.Object);

        // Act
        string actual =
            await converter.ConvertAsync(
                grade,
                sourceSystem);

        // Assert
        Assert.Equal(expected, actual);
    }

    // Boundary / Extreme Values - Ends of the grading scales
    [Theory]
    [InlineData("12", GradeSystem.Danish, "A")]
    [InlineData("-3", GradeSystem.Danish, "F")]
    [InlineData("A", GradeSystem.American, "12")]
    [InlineData("F", GradeSystem.American, "00")]
    public async Task ConvertAsync_ExtremeGradeValues_ReturnsExpectedGrade(
        string grade,
        GradeSystem sourceSystem,
        string expected)
    {
        // Arrange
        Mock<IGradeRepository> repositoryMock = new();

        repositoryMock
            .Setup(repository =>
                repository.GetConvertedGradeAsync(
                    grade,
                    sourceSystem))
            .ReturnsAsync(expected);

        Grade converter =
            new Grade(repositoryMock.Object);

        // Act
        string actual =
            await converter.ConvertAsync(
                grade,
                sourceSystem);

        // Assert
        Assert.Equal(expected, actual);
    }

    // Invalid Equivalence Partition
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ConvertAsync_InvalidGrade_ThrowsArgumentException(
        string? grade)
    {
        // Arrange
        Mock<IGradeRepository> repositoryMock = new();

        Grade converter =
            new Grade(repositoryMock.Object);

        // Act
        Func<Task> act = () =>
            converter.ConvertAsync(
                grade!,
                GradeSystem.Danish);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    // Invalid Grade Not Found
    [Fact]
    public async Task ConvertAsync_GradeNotFound_ThrowsArgumentException()
    {
        // Arrange
        Mock<IGradeRepository> repositoryMock = new();

        repositoryMock
            .Setup(repository =>
                repository.GetConvertedGradeAsync(
                    "99",
                    GradeSystem.Danish))
            .ReturnsAsync((string?)null);

        Grade converter =
            new Grade(repositoryMock.Object);

        // Act
        Func<Task> act = () =>
            converter.ConvertAsync(
                "99",
                GradeSystem.Danish);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(act);
    }

    // Invalid Dependency
    [Fact]
    public void Constructor_NullRepository_ThrowsArgumentNullException()
    {
        // Arrange
        IGradeRepository? repository = null;

        // Act
        Action act = () =>
            new Grade(repository!);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }

    // Dependency Interaction
    [Fact]
    public async Task ConvertAsync_ValidGrade_CallsRepositoryWithCorrectValues()
    {
        // Arrange
        Mock<IGradeRepository> repositoryMock = new();

        repositoryMock
            .Setup(repository =>
                repository.GetConvertedGradeAsync(
                    "12",
                    GradeSystem.Danish))
            .ReturnsAsync("A");

        Grade converter =
            new Grade(repositoryMock.Object);

        // Act
        await converter.ConvertAsync(
            "12",
            GradeSystem.Danish);

        // Assert
        repositoryMock.Verify(
            repository =>
                repository.GetConvertedGradeAsync(
                    "12",
                    GradeSystem.Danish),
            Times.Once);
    }
}
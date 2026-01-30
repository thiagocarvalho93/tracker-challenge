using TrackerApi.DTOs;
using TrackerApi.Models;
using TrackerApi.Repositories;

namespace TrackerApi.Tests.Repositories;

public class PathRepositoryTests : IDisposable
{
    private readonly string _dataDirectory;
    private readonly List<string> _createdFiles = [];

    public PathRepositoryTests()
    {
        _dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");

        if (!Directory.Exists(_dataDirectory))
        {
            Directory.CreateDirectory(_dataDirectory);
        }
    }

    [Fact]
    public async Task GetPathCoordinates_ShouldReturnCoordinates_WhenFileIsValid()
    {
        // Arrange
        var fileName = "valid.csv";
        var filePath = CreateTestFile(fileName,
        [
            "10,20",
            "30,40",
            "50,60"
        ]);

        var repository = new PathRepository();

        // Act
        var result = (await repository.GetPathCoordinates(fileName)).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(new CoordinateDTO(10, 20), result[0]);
        Assert.Equal(new CoordinateDTO(30, 40), result[1]);
        Assert.Equal(new CoordinateDTO(50, 60), result[2]);
    }

    [Fact]
    public async Task GetPathCoordinates_ShouldIgnoreEmptyLines()
    {
        // Arrange
        var fileName = "empty-lines.csv";
        CreateTestFile(fileName,
        [
            "1,2",
            "",
            "   ",
            "3,4"
        ]);

        var repository = new PathRepository();

        // Act
        var result = (await repository.GetPathCoordinates(fileName)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(new CoordinateDTO(1, 2), result[0]);
        Assert.Equal(new CoordinateDTO(3, 4), result[1]);
    }

    [Fact]
    public async Task GetPathCoordinates_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        var repository = new PathRepository();

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(
            () => repository.GetPathCoordinates("missing.csv")
        );
    }

    [Fact]
    public async Task GetPathCoordinates_ShouldThrowFormatException_WhenCsvIsInvalid()
    {
        // Arrange
        var fileName = "invalid.csv";
        CreateTestFile(fileName,
        [
            "10,abc"
        ]);

        var repository = new PathRepository();

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(
            () => repository.GetPathCoordinates(fileName)
        );
    }

    private string CreateTestFile(string fileName, string[] lines)
    {
        var filePath = Path.Combine(_dataDirectory, fileName);
        File.WriteAllLines(filePath, lines);

        _createdFiles.Add(filePath);
        return filePath;
    }

    public void Dispose()
    {
        foreach (var file in _createdFiles)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
        GC.SuppressFinalize(this);
    }
}

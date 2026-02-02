using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Moq;
using TrackerApi.DTOs;
using TrackerApi.Repositories;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Tests.Repositories;

public class PathRepositoryTest : IDisposable
{
    private readonly string _tempRoot;
    private readonly Mock<IWebHostEnvironment> _envMock;
    private readonly IPathRepository _repository;

    public PathRepositoryTest()
    {
        _tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(Path.Combine(_tempRoot, "Data"));

        _envMock = new Mock<IWebHostEnvironment>();
        _envMock.Setup(e => e.ContentRootPath).Returns(_tempRoot);

        _repository = new PathRepository(_envMock.Object);
    }

    [Fact]
    public async Task GetPathCoordinates_ReturnsCoordinates_WhenCsvIsValid()
    {
        // Arrange
        var fileName = "path.csv";
        var filePath = Path.Combine(_tempRoot, "Data", fileName);

        await File.WriteAllLinesAsync(filePath, new[]
        {
        "1,2",
        "3,4",
        "5,6"
    });

        // Act
        var result = (await _repository.GetPathCoordinates(fileName)).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(new CoordinateDTO(1, 2), result[0]);
        Assert.Equal(new CoordinateDTO(3, 4), result[1]);
        Assert.Equal(new CoordinateDTO(5, 6), result[2]);
    }

    [Fact]
    public async Task GetPathCoordinates_IgnoresEmptyLines()
    {
        // Arrange
        var fileName = "path.csv";
        var filePath = Path.Combine(_tempRoot, "Data", fileName);

        await File.WriteAllLinesAsync(filePath, new[]
        {
        "1,2",
        "",
        "   ",
        "3,4"
    });

        // Act
        var result = (await _repository.GetPathCoordinates(fileName)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(new CoordinateDTO(1, 2), result[0]);
        Assert.Equal(new CoordinateDTO(3, 4), result[1]);
    }

    [Fact]
    public async Task GetPathCoordinates_ThrowsFileNotFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        var fileName = "missing.csv";

        // Act
        var exception = await Assert.ThrowsAsync<FileNotFoundException>(
            () => _repository.GetPathCoordinates(fileName)
        );

        // Assert
        Assert.Contains("CSV file not found", exception.Message);
        Assert.EndsWith(Path.Combine("Data", fileName), exception.FileName);
    }

    public void Dispose()
    {
        // Cleanup temp files
        if (Directory.Exists(_tempRoot))
            Directory.Delete(_tempRoot, recursive: true);
    }
}

using Microsoft.Extensions.Options;
using Moq;
using TrackerApi.Configuration;
using TrackerApi.DTOs;
using TrackerApi.Repositories.Interfaces;
using TrackerApi.Services;

namespace TrackerApi.Tests.Services;

public class TrackerServiceTest
{
    private readonly IOptions<PathSettings> _filePathOptions;
    
    public TrackerServiceTest()
    {
        _filePathOptions = Options.Create(new PathSettings
        {
            FileName = "test.csv"
        });
    }
    private static Mock<IPathRepository> CreateRepositoryMock(
        IEnumerable<CoordinateDTO> coordinates)
    {
        var mock = new Mock<IPathRepository>();

        mock.Setup(r => r.GetPathCoordinates(It.IsAny<string>()))
            .ReturnsAsync(coordinates);

        return mock;
    }

    #region GetStatus

    [Fact]
    public async Task GetStatus_ShouldThrow_WhenPathHasLessThanTwoPoints()
    {
        var repoMock = CreateRepositoryMock(
        [
            new CoordinateDTO(0, 0)
        ]);

        var service = new TrackerService(repoMock.Object, _filePathOptions);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetStatus(new CoordinateDTO(1, 1)));
    }

    [Fact]
    public async Task GetStatus_ShouldReturnCorrectOffsetAndStation()
    {
        // Path: (0,0) -> (10,0)
        var repoMock = CreateRepositoryMock(
        [
            new CoordinateDTO(0, 0),
            new CoordinateDTO(10, 0)
        ]);

        var service = new TrackerService(repoMock.Object, _filePathOptions);

        // Point above middle of line
        var result = await service.GetStatus(new CoordinateDTO(5, 3));

        Assert.Equal(3f, result.Offset);
        Assert.Equal(5f, result.Station);
        Assert.Equal(new CoordinateDTO(5, 0), result.ClosestPoint);
    }

    [Fact]
    public async Task GetStatus_ShouldChooseClosestSegment_WhenMultipleSegmentsExist()
    {
        // L shape: (0,0)->(10,0)->(10,10)
        var repoMock = CreateRepositoryMock(
        [
            new CoordinateDTO(0, 0),
            new CoordinateDTO(10, 0),
            new CoordinateDTO(10, 10)
        ]);

        var service = new TrackerService(repoMock.Object, _filePathOptions);

        // Closer to vertical segment
        var result = await service.GetStatus(new CoordinateDTO(12, 6));

        Assert.Equal(2f, result.Offset);
        Assert.Equal(16f, result.Station); // 10 + 6
        Assert.Equal(new CoordinateDTO(10, 6), result.ClosestPoint);
    }

    #endregion

    #region GetStatusWithLineTrack

    [Fact]
    public async Task GetStatusWithLineTrack_ShouldThrow_WhenLineIndexIsInvalid()
    {
        var repoMock = CreateRepositoryMock(
        [
            new CoordinateDTO(0, 0),
        ]);

        var service = new TrackerService(repoMock.Object, _filePathOptions);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.GetStatusWithLineTrack(new CoordinateDTO(5, 1)));
    }

    [Fact]
    public async Task GetStatusWithLineTrack_ShouldReturnCurrentLine_WhenCloserThanNext()
    {
        var repoMock = CreateRepositoryMock(
        [
            new CoordinateDTO(0, 0),
            new CoordinateDTO(10, 0),
            new CoordinateDTO(10, 10)
        ]);

        var service = new TrackerService(repoMock.Object, _filePathOptions);

        var result = await service.GetStatusWithLineTrack(new CoordinateDTO(5, 2));

        Assert.Equal(2f, result.Offset);
        Assert.Equal(5f, result.Station);
        Assert.Equal(0, result.CurrentLineIndex);
        Assert.Equal(new CoordinateDTO(5, 0), result.ClosestPoint);
    }

    [Fact]
    public async Task GetStatusWithLineTrack_ShouldSwitchToNextLine_WhenNextIsCloser()
    {
        var repoMock = CreateRepositoryMock(
        [
            new CoordinateDTO(0, 0),
            new CoordinateDTO(10, 0),
            new CoordinateDTO(10, 10)
        ]);

        var service = new TrackerService(repoMock.Object, _filePathOptions);

        var result = await service.GetStatusWithLineTrack(new CoordinateDTO(11, 6));

        Assert.Equal(1f, result.Offset);
        Assert.Equal(16f, result.Station);
        Assert.Equal(1, result.CurrentLineIndex);
        Assert.Equal(new CoordinateDTO(10, 6), result.ClosestPoint);
    }

    [Fact]
    public async Task GetStatusWithLineTrack_ShouldNotCompareNext_WhenLastLine()
    {
        var repoMock = CreateRepositoryMock(
        [
            new CoordinateDTO(0, 0),
            new CoordinateDTO(10, 0),
            new CoordinateDTO(10, 10)
        ]);

        var service = new TrackerService(repoMock.Object, _filePathOptions);

        var result = await service.GetStatusWithLineTrack(new CoordinateDTO(12, 8));

        Assert.Equal(2f, result.Offset);
        Assert.Equal(18f, result.Station);
        Assert.Equal(1, result.CurrentLineIndex);
        Assert.Equal(new CoordinateDTO(10, 8), result.ClosestPoint);
    }

    #endregion
}

using Microsoft.AspNetCore.Mvc;
using Moq;
using TrackerApi.Controllers;
using TrackerApi.DTOs;
using TrackerApi.Services.Interfaces;

namespace TrackerApi.Tests.Controllers;

public class TrackerControllerTests
{
    private readonly Mock<ITrackerService> _trackerServiceMock;
    private readonly TrackerController _controller;

    public TrackerControllerTests()
    {
        _trackerServiceMock = new Mock<ITrackerService>();
        _controller = new TrackerController(_trackerServiceMock.Object);
    }

    [Fact]
    public async Task GetStatus_WhenTrackLineIsFalse_CallsGetStatus()
    {
        // Arrange
        var coordinate = new CoordinateDTO(1, 2);

        var expectedResponse = new StatusDTO(
            Offset: 10.5,
            Station: 100.0,
            ClosestPoint: new CoordinateDTO(1, 2),
            CurrentLineIndex: 0
        );

        _trackerServiceMock
            .Setup(s => s.GetStatus(coordinate))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetStatus(coordinate, trackLine: false);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResponse, okResult.Value);

        _trackerServiceMock.Verify(s => s.GetStatus(coordinate), Times.Once);
        _trackerServiceMock.Verify(
            s => s.GetStatusWithLineTrack(It.IsAny<CoordinateDTO>(), It.IsAny<int>()),
            Times.Never
        );
    }

    [Fact]
    public async Task GetStatus_WhenTrackLineIsTrue_CallsGetStatusWithLineTrack()
    {
        // Arrange
        var coordinate = new CoordinateDTO(3, 4);
        var currentLineIndex = 2;

        var expectedResponse = new StatusDTO(
            Offset: 15.2,
            Station: 200.0,
            ClosestPoint: new CoordinateDTO(3, 4),
            CurrentLineIndex: currentLineIndex
        );

        _trackerServiceMock
            .Setup(s => s.GetStatusWithLineTrack(coordinate, currentLineIndex))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetStatus(
            coordinate,
            trackLine: true,
            currentLineIndex: currentLineIndex
        );

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedResponse, okResult.Value);

        _trackerServiceMock.Verify(
            s => s.GetStatusWithLineTrack(coordinate, currentLineIndex),
            Times.Once
        );

        _trackerServiceMock.Verify(
            s => s.GetStatus(It.IsAny<CoordinateDTO>()),
            Times.Never
        );
    }

    [Fact]
    public async Task GetPathCoordinates_ReturnsOkWithCoordinates()
    {
        // Arrange
        var expectedCoordinates = new List<CoordinateDTO>
        {
            new(1, 1),
            new(2, 2),
            new(3, 3)
        };

        _trackerServiceMock
            .Setup(s => s.GetPathCoordinates())
            .ReturnsAsync(expectedCoordinates);

        // Act
        var result = await _controller.GetPathCoordinates();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedCoordinates, okResult.Value);

        _trackerServiceMock.Verify(s => s.GetPathCoordinates(), Times.Once);
    }
}
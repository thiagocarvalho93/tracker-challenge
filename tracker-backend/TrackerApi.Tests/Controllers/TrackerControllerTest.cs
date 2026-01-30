namespace TrackerApi.Tests.Controllers;

using Microsoft.AspNetCore.Mvc;
using Moq;
using TrackerApi.Controllers;
using TrackerApi.DTOs;
using TrackerApi.Services.Interfaces;
using Xunit;

public class TrackerControllerTest
{
    private static TrackerController CreateController(
        Mock<ITrackerService> serviceMock)
    {
        return new TrackerController(serviceMock.Object);
    }

    #region GetStatus

    [Fact]
    public async Task GetStatusLess_ShouldReturnOk_WithStatusDTO()
    {
        // Arrange
        var coordinate = new CoordinateDTO(5, 3);
        var expected = new StatusDTO(
            Offset: 3f,
            Station: 5f,
            ClosestPoint: new CoordinateDTO(5, 0));

        var serviceMock = new Mock<ITrackerService>();
        serviceMock
            .Setup(s => s.GetStatus(coordinate))
            .ReturnsAsync(expected);

        var controller = CreateController(serviceMock);

        // Act
        var result = await controller.GetStatusLess(coordinate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult.Value);

        serviceMock.Verify(
            s => s.GetStatus(coordinate),
            Times.Once);
    }

    #endregion

    #region GetStatusStateful

    [Fact]
    public async Task GetStatusFul_ShouldReturnOk_WithStatusStatefulDTO()
    {
        // Arrange
        var coordinate = new CoordinateDTO(10, 6);
        var lineIndex = 1;

        var expected = new StatusStatefulDTO(
            Offset: 2f,
            Station: 16f,
            ClosestPoint: new CoordinateDTO(10, 6),
            CurrentLineIndex: 1);

        var serviceMock = new Mock<ITrackerService>();
        serviceMock
            .Setup(s => s.GetStatusStateful(coordinate, lineIndex))
            .ReturnsAsync(expected);

        var controller = CreateController(serviceMock);

        // Act
        var result = await controller.GetStatusFul(coordinate, lineIndex);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, okResult.Value);

        serviceMock.Verify(
            s => s.GetStatusStateful(coordinate, lineIndex),
            Times.Once);
    }

    #endregion

    #region GetPathCoordinates

    [Fact]
    public async Task GetPathCoordinates_ShouldReturnOk_WithCoordinates()
    {
        // Arrange
        var expected = new[]
        {
            new CoordinateDTO(0, 0),
            new CoordinateDTO(10, 0)
        };

        var serviceMock = new Mock<ITrackerService>();
        serviceMock
            .Setup(s => s.GetPathCoordinates())
            .ReturnsAsync(expected);

        var controller = CreateController(serviceMock);

        // Act
        var result = await controller.GetPathCoordinates();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, okResult.Value);

        serviceMock.Verify(
            s => s.GetPathCoordinates(),
            Times.Once);
    }

    #endregion
}

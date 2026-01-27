using System.Globalization;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories;

public class PathRepository : IPathRepository
{
    public async Task<IEnumerable<Coordinate>> GetPathCoordinates()
    {
        // Read csv file (polyline sample.csv)
        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data",
            "polyline sample.csv"
        );

        if (!File.Exists(filePath))
            throw new FileNotFoundException("CSV file not found", filePath);

        var lines = await File.ReadAllLinesAsync(filePath);

        // Map values to IEnumerable<Coordinate> model
        var coordinates = lines
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line =>
            {
                var values = line.Split(',');

                var x = int.Parse(values[0], CultureInfo.InvariantCulture);
                var y = int.Parse(values[1], CultureInfo.InvariantCulture);
                return new Coordinate(x, y);
            })
            .ToList();

        return coordinates;
    }
}
using System.Globalization;
using TrackerApi.Models;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories;

public class PathRepository : IPathRepository
{
    public async Task<IEnumerable<Coordinate>> GetPathCoordinates()
    {
        string[] lines = await ReadFileAsync();

        List<Coordinate> coordinates = MapLinesToCoordinates(lines);

        return coordinates;
    }

    private static List<Coordinate> MapLinesToCoordinates(string[] lines)
    {
        return lines
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line =>
                    {
                        var values = line.Split(',');

                        var x = int.Parse(values[0], CultureInfo.InvariantCulture);
                        var y = int.Parse(values[1], CultureInfo.InvariantCulture);
                        return new Coordinate(x, y);
                    })
                    .ToList();
    }

    private static async Task<string[]> ReadFileAsync()
    {
        var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Data",
                    "polyline sample.csv"
                );

        if (!File.Exists(filePath))
            throw new FileNotFoundException("CSV file not found", filePath);

        var lines = await File.ReadAllLinesAsync(filePath);
        return lines;
    }
}
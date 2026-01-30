using System.Globalization;
using TrackerApi.DTOs;
using TrackerApi.Repositories.Interfaces;

namespace TrackerApi.Repositories;

public class PathRepository : IPathRepository
{
    public async Task<IEnumerable<CoordinateDTO>> GetPathCoordinates(string fileName)
    {
        string[] lines = await ReadFileAsync(fileName);

        List<CoordinateDTO> coordinates = MapLinesToCoordinates(lines);

        return coordinates;
    }

    private static List<CoordinateDTO> MapLinesToCoordinates(string[] lines)
    {
        return lines
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line =>
                    {
                        var values = line.Split(',');

                        var x = int.Parse(values[0], CultureInfo.InvariantCulture);
                        var y = int.Parse(values[1], CultureInfo.InvariantCulture);
                        return new CoordinateDTO(x, y);
                    })
                    .ToList();
    }

    private static async Task<string[]> ReadFileAsync(string fileName)
    {
        var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Data",
                    fileName
                );

        if (!File.Exists(filePath))
            throw new FileNotFoundException("CSV file not found", filePath);

        var lines = await File.ReadAllLinesAsync(filePath);
        return lines;
    }
}
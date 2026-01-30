namespace TrackerApi.Models;

public record StatusStatefulResponseDTO(double Offset, double Station, Coordinate ClosestPoint, int CurrentLineIndex);
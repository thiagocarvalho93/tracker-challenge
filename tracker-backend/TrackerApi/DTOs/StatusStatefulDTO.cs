namespace TrackerApi.DTOs;

public record StatusStatefulDTO(double Offset, double Station, CoordinateDTO ClosestPoint, int CurrentLineIndex);
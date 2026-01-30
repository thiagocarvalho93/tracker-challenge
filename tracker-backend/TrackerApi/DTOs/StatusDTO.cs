namespace TrackerApi.DTOs;

public record StatusDTO(double Offset, double Station, CoordinateDTO ClosestPoint, int CurrentLineIndex);
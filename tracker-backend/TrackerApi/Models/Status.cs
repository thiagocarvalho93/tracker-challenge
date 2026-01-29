using System.Numerics;

namespace TrackerApi.Models;

public record Status(double Offset, double Station, Coordinate ClosestPoint);
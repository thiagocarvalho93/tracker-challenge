using System.ComponentModel.DataAnnotations;

namespace TrackerApi.DTOs;

public record CoordinateDTO(
    [Required(ErrorMessage = "X is required.")]
    [Range(-float.MaxValue, float.MaxValue, ErrorMessage = "Invalid value for X.")]
    float? X,
    [Required(ErrorMessage = "Y is required.")]
    [Range(-float.MaxValue, float.MaxValue, ErrorMessage = "Invalid value for Y.")]
    float? Y
);
using System.ComponentModel.DataAnnotations;
using _5isen_tracker_dll.Models;

namespace _5isen_tracker_web_app.Models.Ui;

public class WaterContainerCreate
{
    // For now this is what you type manually (later it will come from QR)
    [Required(ErrorMessage = "Device NodeId is required.")]
    [StringLength(16, MinimumLength = 16, ErrorMessage = "NodeId must be exactly 16 characters.")]
    public string NodeId { get; set; } = "";

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = "";

    [Range(1, 10000, ErrorMessage = "Height must be between 1 and 10000 cm.")]
    public decimal HeightCm { get; set; }

    [Required]
    public ContainerShape Shape { get; set; } = ContainerShape.Cylinder;

    // Cylinder
    [Range(0, 10000)]
    public decimal? RadiusCm { get; set; }

    // Rectangular
    [Range(0, 10000)]
    public decimal? LengthCm { get; set; }

    [Range(0, 10000)]
    public decimal? WidthCm { get; set; }

    // CapacityOnly
    [Range(0, 100000)]
    public decimal? MaxLiters { get; set; }
}

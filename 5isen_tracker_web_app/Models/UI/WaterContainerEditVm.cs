using System.ComponentModel.DataAnnotations;
using _5isen_tracker_dll.Models;

namespace _5isen_tracker_web_app.Models.Ui;

public class WaterContainerEditVm
{
    public int Id { get; set; }

    public string QrCode { get; set; } = "";

    [Required]
    public string Name { get; set; } = "";

    [Range(1, 10000)]
    public decimal HeightCm { get; set; }

    public ContainerShape Shape { get; set; }

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

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace _5isen_tracker_dll.Models;

public class WaterContainer
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = default!;


    // Owner (since only 1 user can own a container)
    public int DeviceId { get; set; }
    public Device Device { get; set; }

    // QR value stored on the container
    public string QrCode { get; set; } = default!; // unique

    public string Name { get; set; } = default!;

    // Ultrasonic mounted on top: inside height from sensor to bottom
    public decimal HeightCm { get; set; }

    public ContainerShape Shape { get; set; }

    // If Cylinder
    public decimal? RadiusCm { get; set; }

    // If Rectangular
    public decimal? LengthCm { get; set; }
    public decimal? WidthCm  { get; set; }

    // If CapacityOnly
    public decimal? MaxLiters { get; set; }

}

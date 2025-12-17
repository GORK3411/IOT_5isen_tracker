using System;

namespace _5isen_tracker_dll.Models;

public class Log
{
    public int Id { get; set; }

    public int WaterContainerId { get; set; }
    public WaterContainer WaterContainer { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    
    public decimal DistanceCm { get; set; } // ade masefe ben sensor w l may

    // computed + stored
    public decimal WaterHeightCm { get; set; } // ade fi may en cm
    public decimal WaterPercent { get; set; }
    public decimal WaterLiters { get; set; }
}

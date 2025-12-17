using System;
using _5isen_tracker_dll.Models;

namespace _5isen_tracker_dll.Services;

public static class WaterCalcService
{
    public static (decimal waterHeightCm, decimal percent, decimal liters) Compute(WaterContainer c, decimal distanceCm)
    {
        var rawHeight = c.HeightCm - distanceCm;
        var waterHeight = Clamp(rawHeight, 0m, c.HeightCm);

        var percent = c.HeightCm <= 0m ? 0m : (waterHeight / c.HeightCm) * 100m;
        percent = Clamp(percent, 0m, 100m);

        decimal liters = c.Shape switch
        {
            ContainerShape.Cylinder => CylinderLiters(c, waterHeight),
            ContainerShape.Rectangular => RectangularLiters(c, waterHeight),
            ContainerShape.CapacityOnly => CapacityOnlyLiters(c, percent),
            _ => 0m
        };

        return (Round2(waterHeight), Round2(percent), Round2(liters));
    }

    private static decimal CylinderLiters(WaterContainer c, decimal waterHeight)
    {
        if (c.RadiusCm is null || c.RadiusCm <= 0m) return 0m;

        var r = c.RadiusCm.Value;
        var volumeCm3 = (decimal)Math.PI * r * r * waterHeight; // cm³
        return volumeCm3 / 1000m; // liters
    }

    private static decimal RectangularLiters(WaterContainer c, decimal waterHeight)
    {
        if (c.LengthCm is null || c.WidthCm is null) return 0m;
        if (c.LengthCm <= 0m || c.WidthCm <= 0m) return 0m;

        var volumeCm3 = c.LengthCm.Value * c.WidthCm.Value * waterHeight; // cm³
        return volumeCm3 / 1000m; // liters
    }

    private static decimal CapacityOnlyLiters(WaterContainer c, decimal percent)
    {
        if (c.MaxLiters is null || c.MaxLiters <= 0m) return 0m;
        return (percent / 100m) * c.MaxLiters.Value;
    }

    private static decimal Clamp(decimal v, decimal min, decimal max)
        => v < min ? min : (v > max ? max : v);

    private static decimal Round2(decimal v)
        => Math.Round(v, 2, MidpointRounding.AwayFromZero);
}

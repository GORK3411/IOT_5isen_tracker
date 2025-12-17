using Microsoft.EntityFrameworkCore;
using _5isen_tracker_dll.Data;
using _5isen_tracker_dll.Models;
using _5isen_tracker_dll.Services;

namespace _5isen_tracker_web_app.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(MyApplicationDbContext db)
    {
        // USER
        if (!await db.Users.AnyAsync())
        {
            db.Users.Add(new User
            {
                FullName = "Test User",
                Email = "test@iot.com",
                PasswordHash = "HASH"
            });

            await db.SaveChangesAsync();
        }

        var user = await db.Users.FirstAsync();

        // CONTAINER
        if (!await db.WaterContainers.AnyAsync())
        {
            db.WaterContainers.Add(new WaterContainer
            {
                UserId = user.Id,
                Name = "Roof Tank",
                QrCode = "CNT-0001",
                HeightCm = 150m,
                Shape = ContainerShape.Cylinder,
                RadiusCm = 50m
            });

            await db.SaveChangesAsync();
        }

        var container = await db.WaterContainers.FirstAsync();

        // LOG (one example)
        if (!await db.Logs.AnyAsync())
        {
            var (waterHeight, percent, liters) = WaterCalcService.Compute(container, distanceCm: 40m);

            db.Logs.Add(new Log
            {
                WaterContainerId = container.Id,
                DistanceCm = 40m,
                WaterHeightCm = waterHeight,
                WaterPercent = percent,
                WaterLiters = liters
            });

            await db.SaveChangesAsync();
        }
    }
}

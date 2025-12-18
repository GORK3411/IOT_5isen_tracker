using Microsoft.EntityFrameworkCore;
using _5isen_tracker_dll.Data;
using _5isen_tracker_dll.Models;
using _5isen_tracker_dll.Services;
using Microsoft.AspNetCore.Identity;

namespace _5isen_tracker_dll.Contents;

public static class DbSeeder
{
    public static async Task SeedAsync(MyApplicationDbContext db)
    {
        // USERS
        var users = await db.Users.ToListAsync();
        if (!users.Any())
            return;

        var random = new Random();

        IdentityUser RandomUser() => users[random.Next(users.Count)];

        // =========================
        // DEVICES
        // =========================
        if (!await db.Devices.AnyAsync())
        {
            var devices = new List<Device>
    {
        new Device
        {
            NodeId = "5b6428fd83807f79", // déjà valide (16 chars)
            UserId = RandomUser().Id
        },
        new Device
        {
            NodeId = GenerateNodeId(),
            UserId = RandomUser().Id
        },
        new Device
        {
            NodeId = GenerateNodeId(),
            UserId = RandomUser().Id
        }
    };

            await db.Devices.AddRangeAsync(devices);
            await db.SaveChangesAsync();
        }


        // =========================
        // LOGS
        // =========================
        if (!await db.Logs.AnyAsync())
        {
            // Récupérer tous les devices existants
            var devices = await db.Devices.ToListAsync();
            if (!devices.Any())
                return;

            Device RandomDevice() => devices[random.Next(devices.Count)];

            var logs = new List<Log>();

            // Exemple : 10 logs
            for (int i = 0; i < 10; i++)
            {
                var device = RandomDevice();

                logs.Add(new Log
                {
                    DeviceId = device.Id,
                    DistanceCm = Math.Round(
                        (decimal)(random.NextDouble() * 100 + 10), // 10 → 110 cm
                        2
                    ),
                    CreatedAt = DateTime.UtcNow.AddMinutes(-random.Next(0, 1440)) // dernières 24h
                });
            }

            await db.Logs.AddRangeAsync(logs);
            await db.SaveChangesAsync();
        }

        if (!await db.WaterContainers.AnyAsync())
        {
            var devices = await db.Devices
                .Where(d => d.WaterContainer == null)
                .ToListAsync();

            if (!devices.Any())
                return;



            var containers = devices
                .Take(3) // max 3 containers
                .Select((device, index) => new WaterContainer
                {
                    DeviceId = device.Id,
                    QrCode = Guid.NewGuid().ToString("N"),
                    Name = $"Container {index + 1}",
                    HeightCm = 120 + index * 10,
                    Shape = index % 2 == 0
                        ? ContainerShape.Cylinder
                        : ContainerShape.Rectangular,
                    RadiusCm = index % 2 == 0 ? 35 : null,
                    LengthCm = index % 2 != 0 ? 100 : null,
                    WidthCm = index % 2 != 0 ? 60 : null
                })
                .ToList();

            await db.WaterContainers.AddRangeAsync(containers);
            await db.SaveChangesAsync();
        }

    }

    static string GenerateNodeId()
    {
        return Guid.NewGuid()
            .ToString("N")          // 32 hex chars
            .Substring(0, 16)       // EXACTEMENT 16
            .ToLowerInvariant();
    }


    /*
// CONTAINER
if (!await db.WaterContainers.AnyAsync())
{
    db.WaterContainers.Add(new WaterContainer
    {
        //UserId = user.Id,
        Name = "Roof Tank",
        QrCode = "CNT-0001",
        HeightCm = 150m,
        Shape = ContainerShape.Cylinder,
        RadiusCm = 50m
    });

    await db.SaveChangesAsync();
}

var container = await db.WaterContainers.FirstAsync();
*/
}

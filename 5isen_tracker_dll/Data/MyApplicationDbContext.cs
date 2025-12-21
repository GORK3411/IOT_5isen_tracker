using Microsoft.EntityFrameworkCore;
using _5isen_tracker_dll.Models;

namespace _5isen_tracker_dll.Data;

public class MyApplicationDbContext : DbContext
{
    public MyApplicationDbContext(DbContextOptions<MyApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<WaterContainer> WaterContainers => Set<WaterContainer>();
    public DbSet<Log> Logs => Set<Log>();
    public DbSet<Device> Devices => Set<Device>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // USERS
        /*
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.Id);

            e.Property(x => x.FullName).IsRequired();
            e.Property(x => x.Email).IsRequired();
            e.Property(x => x.PasswordHash).IsRequired();

            e.HasIndex(x => x.Email).IsUnique();
        });
        */

        //DEVICES
        modelBuilder.Entity<Device>(e => {

            e.Property(d => d.NodeId)
         .IsRequired()
         .HasMaxLength(16);

            e.HasCheckConstraint(
                "ck_device_nodeid_len",
                "length(\"NodeId\") = 16"
            );

        });
        // WATER CONTAINERS
        

        modelBuilder.Entity<WaterContainer>(e =>
        {
            e.ToTable("water_containers", t =>
            {
                t.HasCheckConstraint("ck_wc_height_positive", "\"HeightCm\" > 0");

                t.HasCheckConstraint(
                    "ck_wc_dims_by_shape",
                    @"
                    (
                        ""Shape"" = 1 AND ""RadiusCm"" IS NOT NULL
                        AND ""LengthCm"" IS NULL AND ""WidthCm"" IS NULL AND ""MaxLiters"" IS NULL
                    )
                    OR
                    (
                        ""Shape"" = 2 AND ""LengthCm"" IS NOT NULL AND ""WidthCm"" IS NOT NULL
                        AND ""RadiusCm"" IS NULL AND ""MaxLiters"" IS NULL
                    )
                    OR
                    (
                        ""Shape"" = 3 AND ""MaxLiters"" IS NOT NULL
                        AND ""RadiusCm"" IS NULL AND ""LengthCm"" IS NULL AND ""WidthCm"" IS NULL
                    )
                    "
                );
            });
            e.HasKey(x => x.Id);

            e.Property(x => x.QrCode).IsRequired();
            e.HasIndex(x => x.QrCode).IsUnique();

            e.Property(x => x.Name).IsRequired();
            e.Property(x => x.HeightCm).HasPrecision(8, 2).IsRequired();

            e.Property(x => x.Shape).HasConversion<int>().IsRequired();

            e.Property(x => x.RadiusCm).HasPrecision(8, 2);
            e.Property(x => x.LengthCm).HasPrecision(8, 2);
            e.Property(x => x.WidthCm).HasPrecision(8, 2);
            e.Property(x => x.MaxLiters).HasPrecision(10, 2);
            /*
            e.HasOne(x => x.User)
             .WithMany(u => u.WaterContainers)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            */
            //e.HasIndex(x => x.UserId);

            e.HasOne(w => w.Device)
             .WithOne(d => d.WaterContainer)
             .HasForeignKey<WaterContainer>(w => w.DeviceId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(w => w.DeviceId).IsUnique();
        });
        
        // LOGS
        
        modelBuilder.Entity<Log>(e =>
        {
            e.ToTable("logs");
            e.HasKey(x => x.Id);

            e.Property(x => x.CreatedAt).IsRequired();

            e.Property(x => x.DistanceCm).HasPrecision(8, 2).IsRequired();

            e.HasOne(x => x.Device)
            .WithMany(x=>x.Logs)
            .HasForeignKey(x => x.DeviceId);
            /*
            e.Property(x => x.WaterHeightCm).HasPrecision(8, 2).IsRequired();
            e.Property(x => x.WaterPercent).HasPrecision(5, 2).IsRequired();
            e.Property(x => x.WaterLiters).HasPrecision(10, 2).IsRequired();

            e.HasOne(x => x.WaterContainer)
             .WithMany(c => c.Logs)
             .HasForeignKey(x => x.WaterContainerId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => new { x.WaterContainerId, x.CreatedAt });
            */
        });
        
    }
}

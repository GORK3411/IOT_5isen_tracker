using Microsoft.EntityFrameworkCore;
using _5isen_tracker_dll.Data;
using _5isen_tracker_dll.Repositories;
using _5isen_tracker_dll.Repositories.Interfaces;

namespace _5isen_tracker_web_app
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC
            builder.Services.AddControllersWithViews();

            // DB
            var connectionString = builder.Configuration.GetConnectionString("Default");

            builder.Services.AddDbContext<MyApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("5isen_tracker_dll"))
            );

            // Repositories
            builder.Services.AddScoped<IUserRepos, UserRepositories>();
            builder.Services.AddScoped<IWaterContainer, WaterContainerRepositories>();
            builder.Services.AddScoped<ILogRepos, LogRepositories>();

            var app = builder.Build();

            // Optional: seed (custom seeding only)
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<MyApplicationDbContext>();

                // If you have a custom seeder that uses db.Users/db.WaterContainers/db.Logs, call it here:
                // await DbSeeder.SeedAsync(db);
            }

            // Pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // If you later add auth, you'll add app.UseAuthentication() before UseAuthorization()
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}

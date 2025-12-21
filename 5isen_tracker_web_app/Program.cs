using _5isen_tracker_dll.Contents;
using _5isen_tracker_dll.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _5isen_tracker_web_app
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC + Views
            builder.Services.AddControllersWithViews();

            // Identity uses Razor Pages endpoints
            builder.Services.AddRazorPages();

            var connectionString = builder.Configuration.GetConnectionString("Default");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Missing connection string: ConnectionStrings:Default");

            builder.Services.AddDbContext<MyApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("5isen_tracker_dll"))
            );

            builder.Services
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddEntityFrameworkStores<MyApplicationDbContext>();

            var app = builder.Build();

            // ✅ Seed database (only in Development)
            //hone if (app.Environment.IsDevelopment())
            // {
            //     using var scope = app.Services.CreateScope();
            //     var db = scope.ServiceProvider.GetRequiredService<MyApplicationDbContext>();
            //     await DbSeeder.SeedAsync(db);

            //     // seed users (make sure this method is async and awaited)
            //     await UserSeeder.SeeUsersAsync(scope.ServiceProvider);
            // }hone
            if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MyApplicationDbContext>();

    // ✅ This creates/updates tables from your migrations (AspNetUsers etc.)
    await db.Database.MigrateAsync();

    // ✅ Now seed (won’t crash because tables exist)
    await DbSeeder.SeedAsync(db);
    await UserSeeder.SeeUsersAsync(scope.ServiceProvider);
}


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}

using _5isen_tracker_dll.Contents;
using _5isen_tracker_dll.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _5isen_tracker_web_app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("Default");

            builder.Services.AddDbContext<MyApplicationDbContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    b => b.MigrationsAssembly("5isen_tracker_dll")
                )
            );

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            { 
                options.SignIn.RequireConfirmedAccount = false;
            })
                //.AddRoles<IdentityRole>() // we don't have roles
            .AddEntityFrameworkStores<MyApplicationDbContext>();;


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                UserSeeder.SeeUsersAsync(services).Wait();  
            }

                app.Run();
        }
    }
}

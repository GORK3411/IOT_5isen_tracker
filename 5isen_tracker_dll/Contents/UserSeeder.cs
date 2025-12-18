using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5isen_tracker_dll.Contents
{
    public static class UserSeeder
    {
        public static async Task SeeUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            await CreateUser(userManager,"Noel","Noel@mail.com","Noel@123");
            await CreateUser(userManager,"Dia","Dia@mail.com","Dia@123");
            await CreateUser(userManager,"JeanPaul","JeanPaul@mail.com","Jean Paul@123");
        }

        private static async Task CreateUser(UserManager<IdentityUser> userManager,string username,string email,string password)
        {
            if(await userManager.FindByEmailAsync(email)==null)
            {
                var user = new IdentityUser
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = username,
                };

                var result = await userManager.CreateAsync(user,password);
                
            }
        }
    }
}

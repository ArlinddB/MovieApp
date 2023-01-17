using Microsoft.AspNetCore.Identity;
using Redeo.Data.Roles;
using Redeo.Models;

namespace Redeo.Data
{
    public class AppDbInitializer
    {
        public static async Task SeedUsers(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if(!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    
                        await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string username = "admin";
                var adminUser = await userManager.FindByNameAsync(username);
                
                if(adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Redeo Admin",
                        Birthdate = DateTime.Now,
                        UserName = username,
                        Email = "admin@redeo.tv",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "RedeoAdmin1.");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

            }
        }
    }
}

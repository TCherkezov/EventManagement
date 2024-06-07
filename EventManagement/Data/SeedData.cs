using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                
                if (context.Roles.Any())
                {
                    return;   
                }

                var roles = new[]
                {
                    new IdentityRole("Admin"),
                    new IdentityRole("User")
                };

                foreach (var role in roles)
                {
                    await context.Roles.AddAsync(role);
                }
                await context.SaveChangesAsync();

               
                var adminUser = await userManager.FindByEmailAsync("admin@admin.com");
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com"
                    };
                    await userManager.CreateAsync(adminUser, "Admin@123");

                    
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                await context.SaveChangesAsync();
            }
        }
    }
}

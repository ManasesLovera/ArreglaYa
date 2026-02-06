using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Seed
{
    /// <summary>
    /// Database seeder for initializing default data.
    /// </summary>
    public static class DbSeeder
    {
        /// <summary>
        /// Seeds the database with default roles and admin user.
        /// Creates Admin, Client, and Company roles if they don't exist.
        /// Creates a default admin user with all roles if it doesn't exist.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown if admin user creation fails.</exception>
        public static async Task SeedDefaultAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = new[] { "Admin", "Client", "Company" };

            // Ensure roles exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = "admin@arreglaya.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FullName = "Administrator",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(user, "Admin123*_");

                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, roles);
                }
                else
                {
                    throw new Exception("Failed to create default admin user:\n" +
                        string.Join("\n", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}

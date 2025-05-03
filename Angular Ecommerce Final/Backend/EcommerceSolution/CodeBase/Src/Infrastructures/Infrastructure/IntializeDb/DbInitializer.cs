using Application.Ecom.Interfaces;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Infrastructure.IntializeDb
{


    public class DbInitializer : IDbInitializer
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<User> _userManager;

        public DbInitializer(RoleManager<IdentityRole<Guid>> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitializeAsync()
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            var roles = new[] { "Admin", "Customer", "Seller" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            await SeedUserAsync("admin@gmail.com", "Admin@1234", "Admin");
            
        }

        private async Task SeedUserAsync(string email, string password, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User { UserName = email, Email = email, IsVerified = true, CreatedAt=DateTime.UtcNow,UpdatedAt=DateTime.UtcNow };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }

            }
        }
    }
}

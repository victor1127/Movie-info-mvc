using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MovieProDemo.Data;
using MovieProDemo.Enums;
using MovieProDemo.Models;
using MovieProDemo.Services.Interfaces;
using MovieProDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieProDemo.Services
{
    public class SeedService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppSettings _appSettings;

        public SeedService(UserManager<IdentityUser> userManager,
                           ApplicationDbContext context,
                           RoleManager<IdentityRole> roleManager,
                           IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
        }

        public async Task SeedInitialData()
        {
            await UpdateDataBaseAsync();
            await SeedRolesAsync();
            await SeedUserAsync();
            await SeedCollection();
        }

        private async Task UpdateDataBaseAsync()
        {
            await _context.Database.MigrateAsync();
        }
        private async Task SeedRolesAsync()
        {
            foreach(var role in Enum.GetNames(typeof(ApplicationRoles)))
            {
                if (!_context.Roles.Any(r => r.Name == role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        private async Task SeedUserAsync()
        {
            if (_context.Users.Any()) return;

            var credentials = _appSettings.MovieProDemo.DefaultCredentials;
            var administrator = new IdentityUser
            {
                Email = credentials.Email,
                UserName = credentials.Email,
                EmailConfirmed = true
            };
            await _userManager.CreateAsync(administrator, credentials.Password);
            await _userManager.AddToRoleAsync(administrator, credentials.Role);
        }

        private async Task SeedCollection()
        {
            if (_context.Collections.Any()) return;

            var defaulCollection = _appSettings.MovieProDemo.DefaultCollection;
            var collection = new Collection
            {
                Name = defaulCollection.Name,
                Description = defaulCollection.Description
            };

            _context.Add(collection);
            await _context.SaveChangesAsync();
        }
    }
}

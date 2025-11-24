using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ECommerce.Persistence.Data.DataSeed
{
    public class IdentityDataInitializer : IDatainitilizer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataInitializer> _logger;

        public IdentityDataInitializer(UserManager<ApplicationUser> userManager,
                                       RoleManager<IdentityRole> roleManager,
                                       ILogger<IdentityDataInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var adminUser = new ApplicationUser
                    {
                        DisplayName = "Saeed Hosam",
                        UserName = "SaeedHosam",
                        Email = "saeed@gmail.com",
                        PhoneNumber = "01000000000",
                    };
                    var superAdminUser = new ApplicationUser
                    {
                        DisplayName = "Mada",
                        UserName = "MadaMaky",
                        Email = "mada@gmail.com",
                        PhoneNumber = "01000000001",
                    };

                    await _userManager.CreateAsync(adminUser, "P@$$w0rd");
                    await _userManager.CreateAsync(superAdminUser, "P@$$w0rd");

                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding identity data.");
            }
        }
    }
}

using CustomerManagement.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CustomerManagement.Data
{
    public class UserDbSeeder
    {

        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;

        public UserDbSeeder(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedUserAndRolesAndClaims()
        {
            var user = await _userManager.FindByNameAsync("fozgul");
            if (user == null)
            {
                if (!(await _roleManager.RoleExistsAsync("Admin")))
                {
                     var role = new IdentityRole();
                    role.Name = "Admin";

                    await _roleManager.CreateAsync(role);

                    }

                    user = new AppUser()
                    {
                        UserName = "fozgul",
                        FirstName = "Fevzi",
                        LastName = "Ozgul",
                        Email = "f3ozgul@yahoo.com"
                    };
                    var userResult = await _userManager.CreateAsync(user, "P@ssw0rd!");
                    var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
                    var clamResult = await _userManager.AddClaimAsync(user, new Claim("SuperUser", "true"));
                    if (!userResult.Succeeded || !roleResult.Succeeded || !clamResult.Succeeded)
                    {
                        throw new InvalidOperationException("kullanıcı yaratmada sorun var!");
                    }

                }
            }
        }
    }


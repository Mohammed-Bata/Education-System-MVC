using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Utility;

namespace Education_System_MVC_.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager,ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }catch (Exception ex) { }

            if (!_roleManager.RoleExistsAsync(Roles.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Roles.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.Role_Learner)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.Role_Instructor)).GetAwaiter().GetResult();

               var result = _userManager.CreateAsync(new ApplicationUser
                {
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                    Country = "Egypt",
                }, "000000&z").GetAwaiter().GetResult();

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }

                ApplicationUser admin = _db.applicationUsers.FirstOrDefault(u => u.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(admin, Roles.Role_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}

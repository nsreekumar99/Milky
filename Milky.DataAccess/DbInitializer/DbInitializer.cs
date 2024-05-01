using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Milky.DataAccess.Data;
using Milky.Models;
using Milky.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milky.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        //using dependency injection to get dbcontext,roleManager,userManager

        private readonly ApplicationDbContext _db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public void Initialize()
        {
            //Check for pending migrations
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate(); //apply migrations
                }
            }
            catch(Exception ex)
            {

            }

            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult(); //create new roles using utility class
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

                //if roles not created create an admin user aswell
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "nandagopansreekumar98@gmail.com",
                    Email = "nandagopansreekumar98@gmail.com",
                    Name = "Nandagopan Sreekumar",
                    PhoneNumber = "916238025784",
                    Address = "House No 37,neeravil lps nagar,kollam,kerala",
                    City = "Kollam",
                    PostalCode = "691601",
                    State = "Kerala",
                    Street = "Thrikkadavoor",
                }, "Password#$99").GetAwaiter().GetResult();

                //retrieve user object from database
                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "nandagopansreekumar98@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            return;
        }
    }
}

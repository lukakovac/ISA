using ISA.DataAccess.Context;
using ISA.Models;
using Microsoft.AspNetCore.Identity;

namespace ISA.Data
{
    /// <summary>
    /// USED FOR CREATING DEFAULT TEST USERS
    /// TEST USERS FOLLOW THIS PATTERN,
    /// (IE FOR ROLE "TestRole")
    /// Username:       testrole@test.mail
    /// Password:       Testrole2018!
    /// Assigned Role:  TestRole
    /// </summary>
    public class ApplicationDataInitializer
    {
        protected static string[] SYSTEM_ROLES = new[] { "User", "SuperAdmin", "FunZoneAdmin", "MovieAdmin" };


        /// <summary>
        /// USED FOR CREATING DEFAULT TEST USERS
        /// TEST USERS FOLLOW THIS PATTERN,
        /// (IE FOR ROLE "TestRole")
        /// Username:       testrole@test.mail
        /// Password:       Testrole2018!
        /// Assigned Role:  TestRole
        /// </summary>
        public static void SeedData
            (
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager,
                ISAContext context
            )
        {
            SeedEnumerations(context);
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedEnumerations(ISAContext context)
        {
            context.Cinemas.Add(new DataAccess.Models.Cinema
            {
                Address = "Address1",
                Name = "Cinema1",
                Type = DataAccess.Models.Enumerations.CinemaTypeEnum.Cinema
            });
            context.SaveChanges();
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            foreach (var u in SYSTEM_ROLES)
            {
                var username = $"{u.ToLower()}";
                var email = $"{username}@test.mail";
                var pw = $"{char.ToUpper(username[0])}{username.Substring(1)}2018!";

                if (userManager.FindByNameAsync(email).Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                    };

                    IdentityResult result = userManager.CreateAsync(user, pw).Result;

                    if (result.Succeeded)
                    {
                        //add roles to user
                        userManager.AddToRoleAsync(user, u).Wait();
                        //get activation code
                        var code = userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                        //activate user
                        userManager.ConfirmEmailAsync(user, code).Wait();
                    }
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var r in SYSTEM_ROLES)
            {
                if (!roleManager.RoleExistsAsync(r).Result)
                {
                    IdentityRole role = new IdentityRole
                    {
                        Name = r
                    };
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }
    }
}

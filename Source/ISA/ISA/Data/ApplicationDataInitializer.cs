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
        protected static string[] SYSTEM_ROLES = new [] { "User", "SuperAdmin", "FunZoneAdmin", "MovieAdmin" };


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
                RoleManager<IdentityRole> roleManager
            )
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            foreach (var u in SYSTEM_ROLES)
            {
                var username = $"{u.ToLower()}@test.mail";
                var pw = string.Join(string.Empty, char.ToUpper(username[0]), username.Substring(1));

                if (userManager.FindByNameAsync(username).Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = username,
                        Email = username,
                    };

                    IdentityResult result = userManager.CreateAsync(user, pw).Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, u).Wait();
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

using ISA.DataAccess.Context;
using ISA.DataAccess.Models;
using ISA.DataAccess.Models.Enumerations;
using ISA.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

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
            var projection = new Projection
            {
                Description = "desc",
                Duration = new System.TimeSpan(2, 0, 0),
                Name = "Projection 1",
                Type = ProjectionTypeEnum.Movie
            };

            var rep = new Repertoire
            {
                Projections = new List<Projection> { projection }
            };

            var cinema = new Cinema
            {
                Address = "Address1",
                Name = "Cinema1",
                Type = DataAccess.Models.Enumerations.CinemaTypeEnum.Cinema,
                Repertoires = new List<Repertoire> { rep }
            };

            context.Projections.Add(projection);
            context.Repertoires.Add(rep);
            context.Cinemas.Add(cinema);
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

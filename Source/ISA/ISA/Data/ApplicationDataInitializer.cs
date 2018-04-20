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
            if(!context.Database.EnsureCreated())
            {
                SeedEnumerations(context);
            }
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

            var theater = new Theater()
            {
                Name = "Pozoriste Mladih"
            };

            var funZone = new FunZone()
            {
                Cinema = cinema
            };

            var funZoneTheater = new FunZone()
            {
                Theater = theater
            };

            var starWarsProps = new ThematicProps()
            {
                FunZone = funZone,
                Description = "An almost brand new thematic prop for star wars! Used couple of times!",
                Image = @"‪~/images/sw1.jpg",
                Price = 1000,
                Name = "Star wars gear"
            };

            var starTreckProp = new ThematicProps()
            {
                FunZone = funZone,
                Description = "An almost brand new thematic prop for star treck! Used couple of times!",
                Image = @"‪~/images/st1.jpg",
                Price = 2000,
                Name = "Star treck terminal"
            };

            var lotrProp = new ThematicProps()
            {
                FunZone = funZone,
                Description = "An almost brand new thematic prop for LOTR! Used couple of times!",
                Image = @"~/images/lordOfRings_ring.jpg",
                Price = 200000,
                Name = "LOTR ring on sales"
            };

            context.ThematicProps.Add(starWarsProps);
            context.ThematicProps.Add(starTreckProp);
            context.ThematicProps.Add(lotrProp);
            context.Projections.Add(projection);
            context.Repertoires.Add(rep);
            context.Cinemas.Add(cinema);
            context.FunZone.Add(funZone);
            context.FunZone.Add(funZoneTheater);
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

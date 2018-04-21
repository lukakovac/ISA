using ISA.DataAccess.Context;
using ISA.DataAccess.Models;
using ISA.DataAccess.Models.Enumerations;
using ISA.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

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
            SeedUsers(userManager, context);
        }

        private static void SeedEnumerations(ISAContext context)
        {
            for (var i = 0; i < 3; i++)
            {
                var dummyUser = new UserProfile
                {
                    City = $"City{i}",
                    EmailAddress = $"test{i}@test.mail",
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}",
                    TelephoneNr = $"{i}{i}{i}{i}{i}{i}{i}"
                };

                if (context.UserProfiles.All(u => u.EmailAddress != dummyUser.EmailAddress))
                {
                    context.UserProfiles.Add(dummyUser);
                }
            }

            context.SaveChanges();

            var twoUsers = context.UserProfiles.Take(2);
            var firstUser = twoUsers.First();
            var secondUser = twoUsers.Skip(1).Take(1).First();

            if (context.FriendRequests.All(r => r.SenderId != firstUser.Id && r.ReceiverId != secondUser.Id))
            {
                context.FriendRequests.Add(new FriendRequest
                {
                    Sender = firstUser,
                    Receiver = secondUser,
                    Status = FriendshipStatus.Pending
                });
            }

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
                Name = "Star wars gear",
                Publisher = firstUser
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

        public static void SeedUsers(UserManager<ApplicationUser> userManager, ISAContext context)
        {
            foreach (var role in Constants.SYSTEM_ROLES)
            {
                var username = $"{role.ToLower()}";
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
                        userManager.AddToRoleAsync(user, role).Wait();
                        //get activation code
                        var code = userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                        //activate user
                        userManager.ConfirmEmailAsync(user, code).Wait();

                        context.UserProfiles.Add(new UserProfile
                        {
                            City = $"{username} city",
                            EmailAddress = email,
                            FirstName = username,
                            LastName = username,
                            TelephoneNr = email
                        });
                    }
                }
            }
            context.SaveChanges();

            foreach (var role in Constants.SYSTEM_ROLES)
            {
                var username = $"{role.ToLower()}";
                var email = $"{username}@test.mail";
                var up = context.UserProfiles.FirstOrDefault(x => x.EmailAddress == email);
                if (!(up is null))
                {
                    var ur = userManager.FindByNameAsync(email).Result;
                    if (ur != null)
                    {
                        ur.UserProfileId = up.Id;
                        var r = userManager.UpdateAsync(ur).Result;
                    }

                    if (up.Id == 4)
                    {
                        context.FriendRequests.AddRange(new List<FriendRequest>
                        {
                            new FriendRequest
                            {
                                SenderId = 4,
                                Status = FriendshipStatus.Accepted,
                                ReceiverId = 1
                            },

                            new FriendRequest
                            {
                                SenderId = 4,
                                Status = FriendshipStatus.Accepted,
                                ReceiverId = 2,
                            },

                            new FriendRequest
                            {
                                SenderId = 4,
                                Status = FriendshipStatus.Accepted,
                                ReceiverId = 3,
                            },
                        });

                        context.SaveChanges();
                    }
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var r in Constants.SYSTEM_ROLES)
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

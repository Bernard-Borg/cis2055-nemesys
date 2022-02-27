using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Nemesys.Models
{
    //Adapted from what was shown during lectures
    public class DbInitialiser
    {
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole("Reporter")).Wait();
                roleManager.CreateAsync(new IdentityRole("Investigator")).Wait();
                roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
            }
        }

        public static void SeedUsers(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var admin = new User()
                {
                    Email = "bernard@nemesys.com",
                    NormalizedEmail = "BERNARD@NEMESYS.COM",
                    UserName = "bernard@nemesys.com",
                    NormalizedUserName = "BERNARD@NEMESYS.COM",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Alias = "Bernard Borg",
                    Photo = "/images/profileimages/defaultprofile.png",
                    NumberOfReports = 0,
                    NumberOfStars = 0,
                    Bio = "I like to go out for hikes",
                    DateJoined = DateTime.UtcNow,
                    LastActiveDate = DateTime.UtcNow
                };

                IdentityResult result = userManager.CreateAsync(admin, "Random@Password2107").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }

                var admin2 = new User()
                {
                    Email = "nathan@nemesys.com",
                    NormalizedEmail = "NATHAN@NEMESYS.COM",
                    UserName = "nathan@nemesys.com",
                    NormalizedUserName = "NATHAN@NEMESYS.COM",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Alias = "Nathan Portelli",
                    Photo = "/images/profileimages/defaultprofile.png",
                    NumberOfReports = 0,
                    NumberOfStars = 0,
                    DateJoined = DateTime.UtcNow,
                    LastActiveDate = DateTime.UtcNow
                };

                result = userManager.CreateAsync(admin2, "Random@Password2107").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin2, "Admin").Wait();
                }

                var investigator = new User()
                {
                    Email = "andrew@nemesys.com",
                    NormalizedEmail = "ANDREW@NEMESYS.COM",
                    UserName = "andrew@nemesys.com",
                    NormalizedUserName = "ANDREW@NEMESYS.COM",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Alias = "Andrew Caruana",
                    Photo = "/images/profileimages/defaultprofile.png",
                    NumberOfReports = 0,
                    NumberOfStars = 0,
                    Bio = "I like to watch anime and play CSGO",
                    DateJoined = DateTime.UtcNow,
                    LastActiveDate = DateTime.UtcNow
                };

                result = userManager.CreateAsync(investigator, "Random@Password2107").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(investigator, "Investigator").Wait();
                }
                
                var reporter = new User()
                {
                    Email = "bernardcassar@nemesys.com",
                    NormalizedEmail = "BERNARDCASSAR@NEMESYS.COM",
                    UserName = "bernardcassar@nemesys.com",
                    NormalizedUserName = "BERNARDCASSAR@NEMESYS.COM",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Alias = "Bernard Cassar",
                    Photo = "/images/profileimages/defaultprofile.png",
                    NumberOfReports = 3,
                    NumberOfStars = 0,
                    Bio = "I'm a student studying Computing and Business at the University of Malta. I like to play Rocket League",
                    DateJoined = DateTime.UtcNow,
                    LastActiveDate = DateTime.UtcNow
                };

                result = userManager.CreateAsync(reporter, "Random@Password2107").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(reporter, "Reporter").Wait();
                }
            }
        }

        public static void SeedData(UserManager<User> userManager, AppDbContext context)
        {
            if (!context.HazardTypes.Any())
            {
                context.AddRange
                (
                    new HazardType
                    {
                        HazardName = "Unsafe Act"
                    },
                    new HazardType
                    {
                        HazardName = "Condition"
                    },
                    new HazardType
                    {
                        HazardName = "Equipment"
                    },
                    new HazardType
                    {
                        HazardName = "Structure"
                    }
                );
                context.SaveChanges();
            }

            /* 
             * Note that report statuses need to be added in reverse
             * order so that they appear in order in home page
             */

            if (!context.ReportStatuses.Any())
            {
                context.AddRange
                (
                    new ReportStatus
                    {
                        StatusName = "Closed",
                        HexColour = "#DC3545"
                    },
                    new ReportStatus
                    {
                        StatusName = "No Action Required",
                        HexColour = "#17A2B8"
                    },
                    new ReportStatus
                    {
                        StatusName = "Under Investigation",
                        HexColour = "#FFC107"
                    },
                    new ReportStatus
                    {
                        StatusName = "Open",
                        HexColour = "#28A745"
                    }
                );
                context.SaveChanges();
            }

            if (!context.Reports.Any())
            {
                var user = userManager.GetUsersInRoleAsync("Reporter").Result.FirstOrDefault();

                context.AddRange
                (
                    new Report()
                    {
                        DateOfReport = new DateTime(2021, 03, 30),
                        DateOfUpdate = new DateTime(2021, 03, 30),
                        DateTimeOfHazard = new DateTime(2021, 03, 30),
                        Latitude = 35.90346499856232,
                        Longitude = 14.482756912206577,
                        HazardTypeId = 4,
                        Description = "A pothole has appeared inside the Faculty of Education",
                        StatusId = 1,
                        UserId = user.Id,
                        NumberOfStars = 0
                    },
                    new Report()
                    {
                        DateOfReport = new DateTime(2021, 03, 30),
                        DateOfUpdate = new DateTime(2021, 03, 30),
                        DateTimeOfHazard = new DateTime(2021, 03, 30),
                        Latitude = 35.901813789901055,
                        Longitude = 14.485273637516181,
                        HazardTypeId = 3,
                        Description = "The front door has broken in half",
                        StatusId = 1,
                        UserId = user.Id,
                        NumberOfStars = 0
                    },
                    new Report()
                    {
                        DateOfReport = new DateTime(2021, 03, 30),
                        DateOfUpdate = new DateTime(2021, 03, 30),
                        DateTimeOfHazard = new DateTime(2021, 03, 30),
                        Latitude = 35.90342954264499,
                        Longitude = 14.482753809200721,
                        HazardTypeId = 2,
                        Description = "There is a massive puddle",
                        StatusId = 1,
                        UserId = user.Id,
                        Photo = "/images/reportimages/puddle.jpg",
                        NumberOfStars = 0
                    },
                    new Report()
                    {
                        DateOfReport = new DateTime(2021, 03, 30),
                        DateOfUpdate = new DateTime(2021, 03, 30),
                        DateTimeOfHazard = new DateTime(2021, 03, 30),
                        Latitude = 35.80342954264499,
                        Longitude = 14.482753809200721,
                        HazardTypeId = 1,
                        Description = "Someone is driving around in an electric scooter",
                        StatusId = 2,
                        UserId = user.Id,
                        Photo = "/images/reportimages/electric_scooter.jpg",
                        NumberOfStars = 0
                    }
                );

                context.SaveChanges();
            }
        }
    }
}

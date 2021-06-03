using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
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
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Alias = "Bernard Borg",
                    Photo = "/images/defaultprofileblack.png",
                    PhoneNumber = "+35679297880",
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
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Alias = "Nathan Portelli",
                    Photo = "/images/defaultprofileblack.png",
                    PhoneNumber = "+35679333333",
                    NumberOfReports = 0,
                    NumberOfStars = 0,
                    Bio = "I am slowly dying because of some illness",
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
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Alias = "Andrew Caruana",
                    Photo = "/images/defaultprofileblack.png",
                    PhoneNumber = "+35679297079",
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
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Alias = "Bernard Cassar",
                    Photo = "/images/defaultprofileblack.png",
                    PhoneNumber = "+35631415926",
                    NumberOfReports = 0,
                    NumberOfStars = 0,
                    Bio = "I like to play Rocket League",
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
                        DateTimeOfHazard = new DateTime(2021, 03, 30),
                        HazardTypeId = 3,
                        Description = "A Nathan is terrorising the Faculty of ICT",
                        StatusId = 1,
                        UserId = user.Id,
                        NumberOfStars = 0
                    },
                    new Report()
                    {
                        DateOfReport = new DateTime(2021, 03, 30),
                        DateTimeOfHazard = new DateTime(2021, 03, 30),
                        HazardTypeId = 1,
                        Description = "A Kyle is terrorising the Faculty of Education",
                        StatusId = 1,
                        UserId = user.Id,
                        NumberOfStars = 0
                    },
                    new Report()
                    {
                        DateOfReport = new DateTime(2021, 03, 30),
                        DateTimeOfHazard = new DateTime(2021, 03, 30),
                        HazardTypeId = 1,
                        Description = "A massive sinkhole has appeared around the Faculty of Law",
                        StatusId = 2,
                        UserId = user.Id,
                        Photo = "/images/123.png",
                        NumberOfStars = 0,
                        InvestigationId = 1
                    }
                );

                context.SaveChanges();
            }

            if (!context.Investigations.Any())
            {
                var user = userManager.GetUsersInRoleAsync("Investigator").Result.FirstOrDefault();

                context.AddRange
                (
                    new Investigation()
                    {
                        Description = "Hello",
                        ReportId = 1,
                        UserId = user.Id,
                        DateOfAction = DateTime.UtcNow
                    }
                );

                context.SaveChanges();
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Nemesys.Models
{
    public class User : IdentityUser<int>
    {
        //IdentityUser.Id
        //IdentityUser.UserName
        //IdentityUser.Email
        //IdentityUser.PasswordHash
        //IdentityUser.PhoneNumber
        public string Photo { get; set; }
        public UserType TypeOfUser { get; set; }

        public DateTime DateJoined { get; set; }
        public DateTime LastActiveDate { get; set; }

        public int NumberOfReports { get; set; }
        public int NumberOfStars { get; set; }

        public List<Report> Reports { get; set; }
        public List<Report> StarredReports { get; set; }
    }
}
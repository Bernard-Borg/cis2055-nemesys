using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Nemesys.Models
{
    public class User : IdentityUser
    {
        //IdentityUser.Id
        //IdentityUser.UserName
        //IdentityUser.Email
        //IdentityUser.PasswordHash
        //IdentityUser.PhoneNumber
        public string Alias { get; set; }
        public string Bio { get; set; }
        public string Photo { get; set; }

        public DateTime DateJoined { get; set; }
        public DateTime LastActiveDate { get; set; }

        public int NumberOfReports { get; set; }
        public int NumberOfStars { get; set; }

        //Navigation properties
        public List<Report> Reports { get; set; }
        public List<StarRecord> StarredReports { get; set; }
    }
}
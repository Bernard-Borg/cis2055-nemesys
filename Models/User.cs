using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Nemesys.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public string Alias { get; set; }

        [PersonalData]
        public string Bio { get; set; }

        [PersonalData]
        public string Photo { get; set; }

        [PersonalData]
        public DateTime DateJoined { get; set; }
        public DateTime LastActiveDate { get; set; }

        public int NumberOfReports { get; set; }
        public int NumberOfStars { get; set; }

        //Navigation properties
        public List<Report> Reports { get; set; }
        public List<StarRecord> StarredReports { get; set; }
    }
}
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nemesys.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        [MaxLength(20)]
        public string Alias { get; set; }

        [PersonalData]
        [MaxLength(255)]
        public string Bio { get; set; }

        public string Photo { get; set; }

        [PersonalData]
        public DateTime DateJoined { get; set; }

        [PersonalData]
        public DateTime LastActiveDate { get; set; }

        [PersonalData]
        public int NumberOfReports { get; set; }

        [PersonalData]
        public int NumberOfStars { get; set; }

        //Navigation properties
        public List<Report> Reports { get; set; }
        public List<StarRecord> StarredReports { get; set; }
    }
}
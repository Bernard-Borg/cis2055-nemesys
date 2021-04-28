using System;
using System.Collections.Generic;

namespace Nemesys.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public DateTime DateOfReport { get; set; }
        public DateTime DateTimeOfHazard { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public HazardType HazardType { get; set; }
        public string Description { get; set; }
        public ReportStatus Status { get; set; }
        public string Photo { get; set; }

        public int NumberOfStars { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<User> UsersWhichHaveStarred;
    }
}
